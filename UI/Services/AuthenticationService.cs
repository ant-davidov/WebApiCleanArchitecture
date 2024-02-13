using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using MVC.Contracts;
using MVC.Models;
using MVC.Services.Base;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Xml;
using IAuthenticationService = MVC.Contracts.IAuthenticationService;

namespace MVC.Services
{
    public class AuthenticationService : BaseHttpService, IAuthenticationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private JwtSecurityTokenHandler _tokenHandler;
        public AuthenticationService(IHttpContextAccessor httpContextAccessor, IClient client, Contracts.ILocalStorageService localStorage) : base(client, localStorage, httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            _tokenHandler = new JwtSecurityTokenHandler();
        }

        public async Task<bool> Authenticate(string email, string password)
        {
            try
            {
                AuthRequest authRequest = new AuthRequest() { Email = email, Password = password };
                var authResponse = await _client.LoginAsync(authRequest);   
                if (authResponse.Token != String.Empty) 
                {
                    var tokenContent = _tokenHandler.ReadJwtToken(authResponse.Token);
                    var claims = tokenContent.Claims.ToList();
                    claims.Add(new Claim(ClaimTypes.Name, tokenContent.Subject));
                    var user = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));
                    var login = _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, user);
                    var cookieOptions = new CookieOptions
                    {
                        Expires = DateTime.Now.AddHours(1),
                        HttpOnly = true,
                        Secure = true,  // Set the Secure attribute
                       // Adjust SameSite attribute based on your requirements
                    };

                    _httpContextAccessor.HttpContext.Response.Cookies.Append("AuthToken", authResponse.Token, cookieOptions);
                   

                    return true;    
                }
                return false;
            }
            catch (Exception ex) 
            {
                return false;
            }
        }

        

        public async Task Logout()
        {
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(-1),
                HttpOnly = true,
                Secure = true,  // Set the Secure attribute
                                // Adjust SameSite attribute based on your requirements
            };
            _httpContextAccessor.HttpContext.Response.Cookies.Append("AuthToken", String.Empty, cookieOptions);
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public async Task<bool> Register(RegisterVM registration)
        {
            var regRequest = new RegistrationRequest()
            {
                FirstName = registration.FirstName,
                LastName = registration.LastName,
                Email = registration.Email,
                Password = registration.Password,
                UserName = registration.UserName,
            };
            var response = await _client.RegisterAsync(regRequest);
            if(String.IsNullOrEmpty(response.UserId))
                return false;

            await Authenticate(registration.Email, registration.Password);
            return true;
        }
      
    }
}

using Microsoft.AspNetCore.Mvc;
using MVC.Contracts;
using MVC.Models;

namespace MVC.Controllers
{
    public class UsersController : Controller
    {
        private readonly IAuthenticationService _authService;
        private readonly IEmployeeService _employeeService;

        public UsersController(IAuthenticationService authService, IEmployeeService employeeService)
        {
           _authService = authService;
            _employeeService = employeeService;
        }

        public IActionResult Login(string returnUrl = null)
        {
            
            return View();
        }
        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
                return View();
                
            var model = await _employeeService.GetEmployeeAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM login)
        {
          
            if (ModelState.IsValid)
            {
                login.ReturnUrl ??= Url.Content("~/");
                var isLoggedIn = await _authService.Authenticate(login.Email, login.Password);  
                if (isLoggedIn)
                    return LocalRedirect(login.ReturnUrl);
            }
            ModelState.AddModelError("", "Log In Attempt Failed. Please try again.");
            return View(login);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registration)
        {
            if (ModelState.IsValid)
            {
                var returnUrl = Url.Content("~/");
                var isCreated = await _authService.Register(registration);
                if (isCreated)
                    return LocalRedirect(returnUrl);
            }

            ModelState.AddModelError("", "Registration Attempt Failed. Please try again.");
            return View(registration);
        }

        [HttpPost]
        public async Task<IActionResult> Logout(string returnUrl)
        {
            returnUrl ??= Url.Content("~/");
            await _authService.Logout();
            return LocalRedirect(returnUrl);
        }
    }
}

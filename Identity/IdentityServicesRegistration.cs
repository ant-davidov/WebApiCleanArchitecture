using Application.Contracts.Identity;
using Application.Models.Identity;
using Identity.Models;
using Identity.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity
{
    public static class IdentityServicesRegistration
    {
        public static IServiceCollection ConfigureIdentityServices(this IServiceCollection services, WebApplicationBuilder builder)
        {

            if(builder.Environment.IsDevelopment())
            {
                services.AddDbContext<LeaveManagementIdentityDbContext>(options => options.UseSqlite("Data Source=Identity.db"));
            }
            else
            {
                //services.AddDbContext<LeaveManagementIdentityDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
                services.AddDbContext<LeaveManagementIdentityDbContext>(options => options.UseNpgsql(Environment.GetEnvironmentVariable("CONNECTIONSTRINGS__DEFAULT")));
            }


            services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IUserService, UserService>();
            services.AddIdentity<ApplicationUser, IdentityRole>()
             .AddEntityFrameworkStores<LeaveManagementIdentityDbContext>().AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                //  Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
            });
            
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                        ValidAudience = builder.Configuration["JwtSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
                    };
                });
           
            return services;
        }
    }
}

using System.Reflection;
using Humanizer.Inflections;
using Microsoft.AspNetCore.Authentication.Cookies;
using MVC.Contracts;
using MVC.Services;
using MVC.Services.Base;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<CookiePolicyOptions>(opt => opt.MinimumSameSitePolicy = SameSiteMode.None);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = new PathString("/users/login");
            });
builder.Services.AddHttpContextAccessor();

// Add services to the container.
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());    
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<ILocalStorageService,LocalStorageService>();
builder.Services.AddScoped<ILeaveTypeService, LeaveTypeService>();
builder.Services.AddScoped<ILeaveAllocationService, LeaveAllocationService>();
builder.Services.AddScoped<ILeaveRequestService, LeaveRequestService>();
builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();


if(builder.Environment.IsDevelopment())
    builder.Services.AddHttpClient<IClient, Client>(cl => cl.BaseAddress = new Uri("https://localhost:7071"));
else
    builder.Services.AddHttpClient<IClient, Client>(cl => cl.BaseAddress = new Uri(Environment.GetEnvironmentVariable("SERVER_URL")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseCookiePolicy();
app.UseAuthentication();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Users}/{action=Index}/{id?}");

app.Run();

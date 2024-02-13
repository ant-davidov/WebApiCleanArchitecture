using Application.Contracts.Identity;
using Application.Models.Identity;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Employee> GetEmployeeAsync(string userId)
        {
            var employee = await _userManager.FindByIdAsync(userId);
            return new Employee
            {
                Email = employee.Email,
                Id = employee.Id,
                Firstname = employee.FirstName,
                Lastname = employee.LastName
            };
        }
        public async Task<bool> IsAdminAsync(string userId)
        {
            var employee = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(employee);
            return roles.Contains(Roles.Administrator);
        }

        public async Task<List<Employee>> GetEmployeesAsync()
        {
            var employees = await _userManager.GetUsersInRoleAsync(Roles.Employee);
            return employees.Select(q => new Employee
            {
                Id = q.Id,
                Email = q.Email,
                Firstname = q.FirstName,
                Lastname = q.LastName
            }).ToList();
        }
    }
}

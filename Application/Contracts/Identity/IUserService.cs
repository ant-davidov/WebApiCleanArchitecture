using Application.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Identity
{
    public interface IUserService
    {
        Task<Employee> GetEmployeeAsync(string userId);
        Task<bool> IsAdminAsync(string userId);
        Task<List<Employee>> GetEmployeesAsync();
    }
}

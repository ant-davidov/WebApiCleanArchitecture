using MVC.Models;

namespace MVC.Contracts
{
    public interface IEmployeeService
    {
        public Task<EmployeeVM> GetEmployeeAsync();
    }
}

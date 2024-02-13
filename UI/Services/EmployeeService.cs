using AutoMapper;
using MVC.Contracts;
using MVC.Models;
using MVC.Services.Base;

namespace MVC.Services
{
    public class EmployeeService : BaseHttpService, IEmployeeService
    {
        private readonly IMapper _mapper;
        public EmployeeService(IClient client, ILocalStorageService localStorage, IHttpContextAccessor httpContextAccessor, IMapper mapper) : base(client, localStorage, httpContextAccessor)
        {
            _mapper = mapper;
        }

        public async Task<EmployeeVM> GetEmployeeAsync()
        {

            AddBearerToken();
            try
            {
                return _mapper.Map<EmployeeVM>(await _client.EmployeesAsync());
            }
            catch (Exception)
            {

                throw;
            }


        }
    }
}

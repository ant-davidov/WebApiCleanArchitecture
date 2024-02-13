using MVC.Contracts;
using MVC.Services.Base;

namespace MVC.Services
{
    public class LeaveAllocationService : BaseHttpService, ILeaveAllocationService
    {
       
        public LeaveAllocationService(IClient httpclient, ILocalStorageService localStorageService, IHttpContextAccessor httpContextAccessor) : base(httpclient, localStorageService,  httpContextAccessor)
        {
        }

        public async Task<Response<int>> CreateLeaveAllocations(int leaveTypeId)
        {
            try
            {
                var response = new Response<int>();
                CreateLeaveAllocationDTO createLeaveAllocation = new() { LeaveTypeId = leaveTypeId};
                AddBearerToken();
                var apiResponse = await _client.LeaveAllocationsPOSTAsync(createLeaveAllocation);
                if (apiResponse.Success)
                {
                    response.Success = true;
                }
                else
                {
                    foreach (var error in apiResponse.Errors)
                    {
                        response.ValidationErrors += error + Environment.NewLine;
                    }
                }
                return response;
            }
            catch (ApiException ex)
            {
                return ConvertApiExceptions<int>(ex);
            }
        }
    }
}

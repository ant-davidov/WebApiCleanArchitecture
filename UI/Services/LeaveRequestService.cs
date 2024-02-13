using AutoMapper;
using MVC.Contracts;
using MVC.Models;
using MVC.Services.Base;

namespace MVC.Services
{
    public class LeaveRequestService : BaseHttpService, ILeaveRequestService
    {
       
        private readonly IMapper _mapper;
       

        public LeaveRequestService(IMapper mapper, IClient httpclient, ILocalStorageService localStorageService, IHttpContextAccessor httpContextAccessor) : base(httpclient, localStorageService, httpContextAccessor)
        {
          
            _mapper = mapper;
           
        }

        public async Task ApproveLeaveRequest(int id, bool approved)
        {
            AddBearerToken();
            try
            {
                var request = new ChangeLeaveRequestApprovalDTO { Approved = approved, Id = id };
                await _client.ChangeapprovalAsync(id, request);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Response<int>> CreateLeaveRequest(CreateLeaveRequestVM leaveRequest)
        {
            try
            {
                var response = new Response<int>();
                CreateLeaveRequestDTO createLeaveRequest = _mapper.Map<CreateLeaveRequestDTO>(leaveRequest);
                AddBearerToken();
                BaseCommandResponse apiResponse = await _client.LeaveRequestsPOSTAsync(createLeaveRequest);
                if (apiResponse.Success)
                {
                    response.Data = apiResponse.Id;
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

        public Task DeleteLeaveRequest(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<AdminLeaveRequestViewVM> GetAdminLeaveRequestList()
        {
            AddBearerToken();
            var leaveRequests = await _client.LeaveRequestsAllAsync();

            var model = new AdminLeaveRequestViewVM
            {
                TotalRequests = leaveRequests.Count,
                ApprovedRequests = leaveRequests.Count(q => q.Approved == true),
                PendingRequests = leaveRequests.Count(q => q.Approved == null),
                RejectedRequests = leaveRequests.Count(q => q.Approved == false),
                LeaveRequests = _mapper.Map<List<LeaveRequestVM>>(leaveRequests)
            };
            return model;
        }

        public async Task<LeaveRequestVM> GetLeaveRequest(int id)
        {
            AddBearerToken();
            var leaveRequest = await _client.LeaveRequestsGETAsync(id);
            return _mapper.Map<LeaveRequestVM>(leaveRequest);
        }

        public async Task<EmployeeLeaveRequestViewVM> GetUserLeaveRequests()
        {
            AddBearerToken();
            
            var leaveRequests = await _client.LeaveRequestsAllAsync();
            var allocations = await _client.LeaveAllocationsAllAsync(isLoggedInUser: true);
            var model = new EmployeeLeaveRequestViewVM
            {
                LeaveAllocations = _mapper.Map<List<LeaveAllocationVM>>(allocations),
                LeaveRequests = _mapper.Map<List<LeaveRequestVM>>(leaveRequests)
            };

            return model;
        }
    }
}

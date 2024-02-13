using Application.DTOs.LeaveRequest;
using Application.Features.LeaveRequests.Requests.Queries;
using Application.Contracts.Persistence;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Contracts.Identity;
using Domain;
using Microsoft.AspNetCore.Http;
using Application.DTOs.Employee;
using Application.Models.Identity;
using System.Security.Claims;

namespace Application.Features.LeaveRequests.Handlers.Queries
{
    public class GetLeaveRequestListRequestHandler : IRequestHandler<GetLeaveRequestListRequest, List<LeaveRequestListDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public GetLeaveRequestListRequestHandler(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor,IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
        }

        public async Task<List<LeaveRequestListDTO>> Handle(GetLeaveRequestListRequest request, CancellationToken cancellationToken)
        {
            var leaveRequests = new List<LeaveRequest>();
            var requests = new List<LeaveRequestListDTO>();
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(q => q.Type == CustomClaimTypes.Uid)?.Value ?? throw new Exception("no token "); ;
            var logUser= await _userService.GetEmployeeAsync(userId);
                 
            if (await _userService.IsAdminAsync(userId))
            {
               
                leaveRequests = await _unitOfWork.LeaveRequestRepository.GetLeaveRequestsWithDetails();
                requests = _mapper.Map<List<LeaveRequestListDTO>>(leaveRequests);
                foreach (var req in requests)
                {
                    var employee = await _userService.GetEmployeeAsync(req.RequestingEmployeeId);
                    req.Employee = _mapper.Map<EmployeeDTO>(employee);
                }
            }
            else
            {
                leaveRequests = await _unitOfWork.LeaveRequestRepository.GetLeaveRequestsWithDetails(userId);
                var employee = await _userService.GetEmployeeAsync(userId);
                requests = _mapper.Map<List<LeaveRequestListDTO>>(leaveRequests);
                requests.ForEach(req => req.Employee = _mapper.Map<EmployeeDTO>(employee));
            }

            return requests;
        }
    }
}

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
using Application.DTOs.Employee;
using Application.Contracts.Identity;

namespace Application.Features.LeaveRequests.Handlers.Queries
{
    public class GetLeaveRequestDetailRequestHandler : IRequestHandler<GetLeaveRequestDetailRequest, LeaveRequestDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        public GetLeaveRequestDetailRequestHandler(IUnitOfWork leaveRequestRepository, IUserService userService ,IMapper mapper)
        {
            _unitOfWork = leaveRequestRepository;
            _mapper = mapper;
            _userService = userService;
        }
        public async Task<LeaveRequestDTO> Handle(GetLeaveRequestDetailRequest request, CancellationToken cancellationToken)
        {
            var leaveRequest = await _unitOfWork.LeaveRequestRepository.GetLeaveRequestWithDetails(request.Id);
            var dto = _mapper.Map<LeaveRequestDTO>(leaveRequest);
            dto.Employee = _mapper.Map<EmployeeDTO>(await _userService.GetEmployeeAsync(leaveRequest.RequestingEmployeeId));
            return dto;
        }
    }
}

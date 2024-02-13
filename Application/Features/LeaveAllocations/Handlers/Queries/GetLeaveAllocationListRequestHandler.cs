using Application.DTOs.LeaveAllocation;
using Application.Features.LeaveAllocations.Requests.Queries;
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
using Application.Models.Identity;
using Application.DTOs.Employee;
using Application.DTOs.Exceptions;

namespace Application.Features.LeaveAllocations.Handlers.Queries
{
    public class GetLeaveAllocationListRequestHandler : IRequestHandler<GetLeaveAllocationListRequest, List<LeaveAllocationDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;

        public GetLeaveAllocationListRequestHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<LeaveAllocationDTO>> Handle(GetLeaveAllocationListRequest request, CancellationToken cancellationToken)
        {
            var leaveAllocations = new List<LeaveAllocation>();
            var allocations = new List<LeaveAllocationDTO>();
            var a = _httpContextAccessor.HttpContext;
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(q => q.Type == CustomClaimTypes.Uid)?.Value ?? throw new BadRequestException("no token");
          
            if (await _userService.IsAdminAsync(userId))
            {
               
                leaveAllocations = await _unitOfWork.LeaveAllocationRepository.GetLeaveAllocationsWithDetails();
                allocations = _mapper.Map<List<LeaveAllocationDTO>>(leaveAllocations);
               
            }
            else
            {
               
                leaveAllocations = await _unitOfWork.LeaveAllocationRepository.GetLeaveAllocationsWithDetails(userId);
                var employee = await _userService.GetEmployeeAsync(userId);
                allocations = _mapper.Map<List<LeaveAllocationDTO>>(leaveAllocations);
               
            }

            return allocations;
        }
    }
}

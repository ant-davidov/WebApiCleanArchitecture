using Application.DTOs.Exceptions;
using Application.DTOs.LeaveAllocation.Validators;
using Application.Features.LeaveAllocations.Requests.Commands;
using Application.Contracts.Persistence;
using AutoMapper;
using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Responses;
using Application.Contracts.Identity;
using Microsoft.AspNetCore.Http;
using Application.Models.Identity;
using System.Security.Claims;


namespace Application.Features.LeaveAllocations.Handlers.Commands
{
    public class CreateLeaveAllocationCommandHandler : IRequestHandler<CreateLeaveAllocationCommand, BaseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public CreateLeaveAllocationCommandHandler(
           IUnitOfWork unitOfWork,
            IUserService userService,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<BaseCommandResponse> Handle(CreateLeaveAllocationCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var validator = new CreateLeaveAllocationDtoValidator(_unitOfWork.LeaveTypeRepository);
            var validationResult = await validator.ValidateAsync(request.CreateLeaveAllocationDTO);

            if (validationResult.IsValid == false)
            {
                response.Success = false;
                response.Message = "Allocations Failed";
                response.Errors = validationResult.Errors.Select(q => q.ErrorMessage).ToList();
            }
            else
            {
                var leaveType = await _unitOfWork.LeaveTypeRepository.GetAsync(request.CreateLeaveAllocationDTO.LeaveTypeId);
                var employees = await _userService.GetEmployeesAsync();
                var period = DateTime.Now.Year;
                var allocations = new List<LeaveAllocation>();
                foreach (var emp in employees)
                {
                    if (await _unitOfWork.LeaveAllocationRepository.AllocationExists(emp.Id, leaveType.Id, period))
                        continue;
                    allocations.Add(new LeaveAllocation
                    {
                        EmployeeId = emp.Id,
                        LeaveTypeId = leaveType.Id,
                        NumberOfDays = leaveType.DefaultDays,
                        Period = period
                    });
                }
                await _unitOfWork.LeaveAllocationRepository.AddAllocations(allocations);     
                await _unitOfWork.SaveAsync();
                response.Success = true;
                response.Message = "Allocations Successful";
            }


            return response;
        }
    }
}

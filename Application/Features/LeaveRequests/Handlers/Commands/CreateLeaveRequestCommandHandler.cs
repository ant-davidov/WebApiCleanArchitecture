using Application.DTOs.Exceptions;
using Application.DTOs.LeaveRequest;
using Application.DTOs.LeaveRequest.Validators;
using Application.Features.LeaveRequests.Requests.Commands;
using Application.Contracts.Persistence;
using AutoMapper;
using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Contracts.Infrastructure;
using Application.Models;
using Application.Responses;
using Microsoft.AspNetCore.Http;
using Application.Models.Identity;
using Application.DTOs.LeaveAllocation;
using System.Security.Claims;

namespace Application.Features.LeaveRequests.Handlers.Commands
{
    public class CreateLeaveRequestCommandHandler : IRequestHandler<CreateLeaveRequestCommand, BaseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationSender _notificationSender;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public CreateLeaveRequestCommandHandler(
            IUnitOfWork unitOfWork,
            INotificationSender notificationSender,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            _notificationSender = notificationSender;
            this._httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<BaseCommandResponse> Handle(CreateLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var validator = new CreateLeaveRequestDtoValidator();
            var validationResult = await validator.ValidateAsync(request.CreateLeaveRequestDTO);
            var userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(
                    q => q.Type == CustomClaimTypes.Uid)?.Value;

            var allocation = await _unitOfWork.LeaveAllocationRepository.GetUserAllocations(userId, request.CreateLeaveRequestDTO.LeaveTypeId);
            if (allocation is null)
            {
                validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure(nameof(request.CreateLeaveRequestDTO.LeaveTypeId),
                    "You do not have any allocations for this leave type."));
            }
            else
            {
                int daysRequested = (int)(request.CreateLeaveRequestDTO.EndDate - request.CreateLeaveRequestDTO.StartDate).TotalDays;
                if (daysRequested > allocation.NumberOfDays)
                {
                    validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure(
                        nameof(request.CreateLeaveRequestDTO.EndDate), "You do not have enough days for this request"));
                }
            }

            if (validationResult.IsValid == false)
            {
                response.Success = false;
                response.Message = "Request Failed";
                response.Errors = validationResult.Errors.Select(q => q.ErrorMessage).ToList();
            }
            else
            {
                var leaveRequest = _mapper.Map<LeaveRequest>(request.CreateLeaveRequestDTO);
                leaveRequest.RequestingEmployeeId = userId;
                leaveRequest.DateRequested = DateTime.UtcNow;
                leaveRequest = await _unitOfWork.LeaveRequestRepository.AddAsync(leaveRequest);
                await _unitOfWork.SaveAsync();

                response.Success = true;
                response.Message = "Request Created Successfully";
                response.Id = leaveRequest.Id;

                try
                {
                    var emailAddress = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email).Value;

                    var email = new Email
                    {
                        To = emailAddress,
                        Body = $"Your leave request for {request.CreateLeaveRequestDTO.StartDate:D} to {request.CreateLeaveRequestDTO.EndDate:D} " +
                        $"has been submitted successfully.",
                        Subject = "Leave Request Submitted"
                    };

                     _notificationSender.SendMessage(email);
                }
                catch (Exception ex)
                {
                    //// Log or handle error, but don't throw...
                }
            }

            return response;
        }
    }
}

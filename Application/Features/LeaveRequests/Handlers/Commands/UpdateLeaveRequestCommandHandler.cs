using Application.DTOs.Exceptions;
using Application.DTOs.LeaveRequest.Validators;
using Application.Features.LeaveRequests.Requests.Commands;
using Application.Contracts.Persistence;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.LeaveRequests.Handlers.Commands
{
    public class UpdateLeaveRequestCommandHandler : IRequestHandler<UpdateLeaveRequestCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateLeaveRequestCommandHandler(
            IUnitOfWork unitOfWork,
             IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var leaveRequest = await _unitOfWork.LeaveRequestRepository.GetAsync(request.Id);

            if (leaveRequest is null)
                throw new NotFoundException(nameof(leaveRequest), request.Id);

            if (request.LeaveRequestDTO != null)
            {
                var validator = new UpdateLeaveRequestDtoValidator();
                var validationResult = await validator.ValidateAsync(request.LeaveRequestDTO);
                if (validationResult.IsValid == false)
                    throw new ValidationException(validationResult);

                _mapper.Map(request.LeaveRequestDTO, leaveRequest);

                await _unitOfWork.LeaveRequestRepository.UpdateAsync(leaveRequest);
                await _unitOfWork.SaveAsync();
            }
            else if (request.ChangeLeaveRequestApprovalDTO != null)
            {
                await _unitOfWork.LeaveRequestRepository.ChangeApprovalStatus(leaveRequest, request.ChangeLeaveRequestApprovalDTO.Approved);
                if (request.ChangeLeaveRequestApprovalDTO.Approved)
                {
                    var allocation = await _unitOfWork.LeaveAllocationRepository.GetUserAllocations(leaveRequest.RequestingEmployeeId, leaveRequest.LeaveTypeId);
                    int daysRequested = (int)(leaveRequest.EndDate - leaveRequest.StartDate).TotalDays;
                    if(allocation.NumberOfDays < daysRequested)
                        throw new BadRequestException("You do not have enough days for this request");

                    allocation.NumberOfDays -= daysRequested;

                    await _unitOfWork.LeaveAllocationRepository.UpdateAsync(allocation);
                }

                await _unitOfWork.SaveAsync();
            }

            return Unit.Value;
        }
    }
}

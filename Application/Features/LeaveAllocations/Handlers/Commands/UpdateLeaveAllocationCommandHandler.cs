using Application.DTOs.Exceptions;
using Application.DTOs.LeaveAllocation.Validators;
using Application.Features.LeaveAllocations.Requests.Commands;
using Application.Contracts.Persistence;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.LeaveAllocations.Handlers.Commands
{
    public class UpdateLeaveAllocationCommandHandler : IRequestHandler<UpdateLeaveAllocationCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateLeaveAllocationCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateLeaveAllocationCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdateLeaveAllocationDtoValidator();
            var validationResult = await validator.ValidateAsync(request.LeaveAllocationDTO);

            if (validationResult.IsValid == false)
                throw new ValidationException(validationResult);

            var leaveAllocation = await _unitOfWork.LeaveAllocationRepository.GetAsync(request.LeaveAllocationDTO.Id);

            if (leaveAllocation is null)
                throw new NotFoundException(nameof(leaveAllocation), request.LeaveAllocationDTO.Id);

            _mapper.Map(request.LeaveAllocationDTO, leaveAllocation);
            await _unitOfWork.LeaveAllocationRepository.UpdateAsync(leaveAllocation);
            await _unitOfWork.SaveAsync();
            return Unit.Value;
        }
    }
}

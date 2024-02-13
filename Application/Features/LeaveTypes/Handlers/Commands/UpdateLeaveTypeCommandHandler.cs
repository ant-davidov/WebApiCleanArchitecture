using Application.DTOs.Exceptions;
using Application.DTOs.LeaveType.Validators;
using Application.Features.LeaveTypes.Requests.Commands;
using Application.Contracts.Persistence;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Application.DTOs.LeaveType.Validators.UpdateLeaveTypeDTOValidator;
using Domain;

namespace Application.Features.LeaveTypes.Handlers.Commands
{
    public class UpdateLeaveTypeCommandHandler : IRequestHandler<UpdateLeaveTypeCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateLeaveTypeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateLeaveTypeCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdateLeaveTypeDtoValidator();
            var validationResult = await validator.ValidateAsync(request.LeaveTypeDTO);

            if (validationResult.IsValid == false)
                throw new ValidationException(validationResult);

            var leaveType = await _unitOfWork.LeaveTypeRepository.GetAsync(request.LeaveTypeDTO.Id) ?? throw new NotFoundException(nameof(LeaveType), request.LeaveTypeDTO.Id); ;


            _mapper.Map(request.LeaveTypeDTO, leaveType);

            await _unitOfWork.LeaveTypeRepository.UpdateAsync(leaveType);
            await _unitOfWork.SaveAsync();

            return Unit.Value;
        }
    }
}

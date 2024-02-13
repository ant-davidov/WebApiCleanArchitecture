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
using Application.DTOs.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Application.Features.LeaveAllocations.Handlers.Commands
{
    public class DeleteLeaveAllocationCommandHandler : IRequestHandler<DeleteLeaveAllocationCommand,Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public DeleteLeaveAllocationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(DeleteLeaveAllocationCommand request, CancellationToken cancellationToken)
        {
            var leaveAllocation = await _unitOfWork.LeaveAllocationRepository.GetAsync(request.Id);

            if (leaveAllocation == null)
                throw new NotFoundException(nameof(LeaveAllocation), request.Id);

            await _unitOfWork.LeaveAllocationRepository.DeleteAsync(leaveAllocation);
            await _unitOfWork.SaveAsync();
            return Unit.Value;
        }
    }
}

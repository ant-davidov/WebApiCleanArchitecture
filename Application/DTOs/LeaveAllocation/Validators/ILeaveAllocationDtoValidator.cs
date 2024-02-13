using Application.Contracts.Persistence;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.LeaveAllocation.Validators
{
    public class ILeaveAllocationDtoValidator : AbstractValidator<ILeaveAllocationDTO>
    {
       

        public ILeaveAllocationDtoValidator()
        {
          
            RuleFor(p => p.LeaveTypeId)
                .GreaterThan(0)
                .WithMessage("{PropertyName} does not exist.");

        }
    }
}

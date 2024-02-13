using Application.Contracts.Persistence;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.LeaveAllocation.Validators
{
    public class UpdateLeaveAllocationDtoValidator : AbstractValidator<UpdateLeaveAllocationDTO>
    {
        

        public UpdateLeaveAllocationDtoValidator()
        {
           
            RuleFor(p => p.Id).GreaterThan(0).WithMessage("id invalid");
           

            
        }
    }
}

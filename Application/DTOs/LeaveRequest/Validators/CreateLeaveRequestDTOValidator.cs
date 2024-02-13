using Application.Contracts.Persistence;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.LeaveRequest.Validators
{
    public class CreateLeaveRequestDtoValidator : AbstractValidator<CreateLeaveRequestDTO>
    {
       

        public CreateLeaveRequestDtoValidator()
        {
           
            Include(new ILeaveRequestDtoValidator());
           
        }
    }
}

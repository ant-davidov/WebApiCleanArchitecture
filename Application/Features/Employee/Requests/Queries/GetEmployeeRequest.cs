using Application.DTOs.Employee;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Employee.Requests.Queries
{
    public class GetEmployeeRequest : IRequest<EmployeeDetailDTO>
    {
        public string Guid { get; set; }
    }
}

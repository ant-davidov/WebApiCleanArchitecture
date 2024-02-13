using Application.Contracts.Identity;
using Application.DTOs.Employee;
using Application.Features.Employee.Requests.Queries;
using Application.Models.Identity;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApiCleanArchitecture.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public EmployeesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<EmployeeDetailDTO>> GetUser()
        {
            return await _mediator.Send(new GetEmployeeRequest());
        }
    }
}

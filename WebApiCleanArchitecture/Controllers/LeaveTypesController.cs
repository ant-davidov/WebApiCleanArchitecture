using Application.DTOs.LeaveType;
using Application.Features.LeaveAllocations.Requests.Queries;
using Application.Features.LeaveTypes.Handlers.Queries;
using Application.Features.LeaveTypes.Requests.Commands;
using Application.Features.LeaveTypes.Requests.Queries;
using Application.Responses;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiCleanArchitecture.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveTypesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public LeaveTypesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<LeaveTypeDTO>>> Get()
        {
            var leaveTypes = await _mediator.Send(new GetLeaveTypeListRequest());
            return leaveTypes;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LeaveTypeDTO>> Get(int id)
        {
            var leaveTypes =  await _mediator.Send(new GetLeaveTypeDetailRequest { Id = id});
            return leaveTypes; 
        }

        [HttpPost]
        public async Task<ActionResult<BaseCommandResponse>> Post([FromBody] CreateLeaveTypeDTO createLeaveType)
        {
            var response = await _mediator.Send(new CreateLeaveTypeCommand { LeaveTypeDTO = createLeaveType });
            return response;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] LeaveTypeDTO leaveType)
        {
            var command  = new UpdateLeaveTypeCommand {  LeaveTypeDTO = leaveType };
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var command = new DeleteLeaveTypeCommand {  Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}

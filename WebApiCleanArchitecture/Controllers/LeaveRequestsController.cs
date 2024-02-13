using Application.DTOs.LeaveRequest;
using Application.Features.LeaveRequests.Requests.Commands;
using Application.Features.LeaveRequests.Requests.Queries;
using Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApiCleanArchitecture.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class LeaveRequestsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LeaveRequestsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<LeaveRequestListDTO>>> Get()
        {
            var leaveRequests = await _mediator.Send(new GetLeaveRequestListRequest() );
            return Ok(leaveRequests);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LeaveRequestDTO>> Get(int id)
        {
            var leaveRequest = await _mediator.Send(new GetLeaveRequestDetailRequest { Id = id });
            return Ok(leaveRequest);
        }

        [HttpPost]
        public async Task<ActionResult<BaseCommandResponse>> Post([FromBody] CreateLeaveRequestDTO leaveRequest)
        {
            var command = new CreateLeaveRequestCommand { CreateLeaveRequestDTO = leaveRequest };
            var repsonse = await _mediator.Send(command);
            return Ok(repsonse);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] UpdateLeaveRequestDTO leaveRequest)
        {
            var command = new UpdateLeaveRequestCommand { Id = id, LeaveRequestDTO = leaveRequest };
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPut("changeapproval/{id}")]
        public async Task<ActionResult> ChangeApproval(int id, [FromBody] ChangeLeaveRequestApprovalDTO changeLeaveRequestApproval)
        {
            var command = new UpdateLeaveRequestCommand { Id = id, ChangeLeaveRequestApprovalDTO = changeLeaveRequestApproval };
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var command = new DeleteLeaveRequestCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RSVP.Application.Dtos;
using RSVP.Application.Features.Attendie.Command.AddAttendie;
using RSVP.Application.Features.Attendie.Command.AddAttendieByGroup;
using RSVP.Application.Features.Attendie.Command.DeleteAttendie;
using RSVP.Application.Features.Attendie.Command.UpdateAttendie;
using RSVP.Application.Features.Attendie.Queries.GetAttendiesViaEventFIlter;

namespace RSVP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AttendieController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AttendieController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<int>> AddAttendie([FromBody] AddAttendieCommand request)
        {
            int result = await _mediator.Send(request);
            return CreatedAtAction(nameof(AddAttendie), new { id = result }, result);
        }

        [HttpPost("bulk")]
        public async Task<ActionResult<bool>> AddAttendieByGroup([FromBody] AddAttendieByGroupCommand request)
        {
            bool result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpPut("{attendieId}")]
        public async Task<ActionResult<bool>> UpdateAttendie(int attendieId, [FromBody] UpdateAttendieCommand request)
        {
            request.AttendieId = attendieId;
            bool result = await _mediator.Send(request);
            return NoContent();
        }

        [HttpDelete("{attendieId}")]
        public async Task<ActionResult<bool>> DeleteAttendie(int attendieId)
        {
            var request = new DeleteAttendieCommand { AttendieId = attendieId };
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpGet("event/{eventId}")]
        public async Task<ActionResult<GetAttendiesViaEventFilterResponseDto>> GetAttendiesByEvent(
            int eventId, 
            [FromQuery] GetAttendiesViaEventFIlterQuery request)
        {
            request.EventId = eventId;
            GetAttendiesViaEventFilterResponseDto result = await _mediator.Send(request);
            return Ok(result);
        }
    }
}

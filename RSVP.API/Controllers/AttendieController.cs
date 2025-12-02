using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RSVP.Application.Dtos;
using RSVP.Application.Features.Attendie.Command.AddAttendie;
using RSVP.Application.Features.Attendie.Command.AddAttendieByGroup;
using RSVP.Application.Features.Attendie.Command.DeleteAttendie;
using RSVP.Application.Features.Attendie.Command.UpdateAttendie;
using RSVP.Application.Features.Attendie.Queries.GetAttendiesViaEventFIlter;
using RSVP.Application.Features.Attendie.Queries.GetAttendieViaUserid;
using appDomain = RSVP.Domain.Entities;
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
        [HttpGet("user")]
        public async Task<ActionResult<appDomain.Attendie>> GetAttendieViaUserId([FromQuery] int userId, [FromQuery] int eventId)
        {
            GetAttendieViaUseridQuery query = new GetAttendieViaUseridQuery { UserId = userId, EventId = eventId };
            var attendie = await _mediator.Send(query);
            if (attendie is null)
            {
                return NotFound();
            }
            return Ok(attendie);
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

        [HttpPatch("{attendieId}")]
        public async Task<ActionResult<bool>> UpdateAttendie(int attendieId,  UpdateAttendieCommand request)
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

        [HttpPost("filter")]
        public async Task<ActionResult<GetAttendiesViaEventFilterResponseDto>> GetAttendiesByEvent(
            [FromBody] GetAttendiesViaEventFIlterQuery request)
        {
            
            GetAttendiesViaEventFilterResponseDto result = await _mediator.Send(request);
            return Ok(result);
        }
    }
}

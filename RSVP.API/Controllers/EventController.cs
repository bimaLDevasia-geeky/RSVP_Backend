using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RSVP.Application.Features.Event.Commands.CreateEvent;
using RSVP.Application.Features.Event.Commands.DeleteEvent;
using RSVP.Application.Features.Event.Commands.UpdateEvent;
using RSVP.Application.Features.Event.Queries.GetEventById;
using RSVP.Application.Features.Event.Queries.GetEventOrgOrOwn;
using RSVP.Application.Features.Event.Queries.GetInvitedEvents;
using appDomain = RSVP.Domain.Entities;

namespace RSVP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EventController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EventController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateEvent([FromBody] CreateEventCommand request)
        {
             int result = await _mediator.Send(request);
            return CreatedAtAction(nameof(GetEventById), new { id = result }, result);
        }

        [HttpPut("{eventId}")]
        public async Task<ActionResult<bool>> UpdateEvent(int eventId, [FromBody] UpdateEventCommand request)
        {
            request.EventId = eventId;
             bool result = await _mediator.Send(request);
            return NoContent();
        }

        [HttpDelete("{eventId}")]
        public async Task<ActionResult<bool>> DeleteEvent(int eventId)
        {
            DeleteEventCommand request = new DeleteEventCommand { EventId = eventId };
            bool result = await _mediator.Send(request);
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<appDomain.Event>> GetEventById(int id)
        {
             GetEventByIdQuery request = new GetEventByIdQuery { Id = id };
            appDomain.Event result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpGet("my-events")]
        public async Task<ActionResult<List<appDomain.Event>>> GetMyEvents()
        {
             GetEventOrgOrOwnQuery request = new GetEventOrgOrOwnQuery();
            List<appDomain.Event> result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpGet("invited")]
        public async Task<ActionResult<List<appDomain.Event>>> GetInvitedEvents()
        {
             GetInvitedEventQuery request = new GetInvitedEventQuery();
            List<appDomain.Event> result = await _mediator.Send(request);
            return Ok(result);
        }
    }
}

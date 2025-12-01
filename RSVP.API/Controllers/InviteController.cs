using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RSVP.Application.Dtos;
using RSVP.Application.Features.Invite.Commands;
using RSVP.Application.Features.Invite.Queries;

namespace RSVP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InviteController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InviteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("invite")]
        public async Task<ActionResult<bool>> Invite([FromBody] AcceptInviteCommand request)
        {
            bool result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpGet("get-event/{inviteCode}")]
        public async Task<ActionResult<EventDto>> GetEventByInviteCode(string inviteCode)
        {
            var query = new GetEventByInviteIdQuery { InviteCode = inviteCode };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}

using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RSVP.Application.Features.Request.Commands.CreateRequest;
using RSVP.Application.Features.Request.Commands.UpdateRequest;
using RSVP.Domain.Enums;

namespace RSVP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        
        private readonly IMediator _mediator;

        public RequestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRequest([FromBody] CreateRequestCommand command)
        {
            var requestId = await _mediator.Send(command);
            return Ok(requestId);
        }


        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateRequestStatus(int id,UpdateRequestCommand command)
        {
            command.Id = id;

            await _mediator.Send(command);
            return NoContent();
        }
    }
}

using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RSVP.Application.Features.Request.Commands.CreateRequest;
using RSVP.Application.Features.Request.Commands.UpdateRequest;
using RSVP.Application.Features.Request.Queries.GetRequestsByEvent;
//using RSVP.Application.Features.Request.Queries.GetRequestsByUser;
using RSVP.Domain.Enums;

namespace RSVP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExternalRequestController : ControllerBase
    {
        
        private readonly IMediator _mediator;

        public ExternalRequestController(IMediator mediator)
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


        // [HttpGet("by-user")]
        // public async Task<IActionResult> GetRequestsByUser()
        // {
        //     var query = new GetRequestsByUserQuery();
        //     var requests = await _mediator.Send(query);
        //     return Ok(requests);
        // }


        [HttpGet("event/{eventId}")]
        public async Task<IActionResult> GetRequestsByEvent(int eventId)
        {
            var query = new GetRequestsByEventQuery
            {
                EventId = eventId
            };
            var requests = await _mediator.Send(query);
            return Ok(requests);
        }
    }
}

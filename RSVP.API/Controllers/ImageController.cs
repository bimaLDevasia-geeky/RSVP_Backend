using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RSVP.Application.Features.Images.Commands.AddImage;
using RSVP.Application.Features.Images.Commands.AddImageInGroup;
using RSVP.Application.Features.Images.Commands.DeleteImage;
using RSVP.Application.Features.Images.Commands.DeleteImagesInGroup;
using RSVP.Application.Features.Images.Query.GetImagesOfEvent;
using RSVP.Domain.Entities;

namespace RSVP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ImageController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ImageController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<bool>> AddImage([FromBody] AddImageCommand request)
        {
            bool result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpPost("bulk")]
        public async Task<ActionResult<bool>> AddImageInGroup([FromBody] AddImageInGroupCommand request)
        {
            bool result = await _mediator.Send(request);
            return NoContent();
        }
        [HttpDelete("bulk")]
        public async Task<IActionResult>leteImagesInGroup([FromBody] DeleteImagesInGroupCommand request)
        {
             bool result = await _mediator.Send(request);
                return NoContent();
        }

        [HttpDelete("{imageId}")]
        public async Task<ActionResult<bool>> DeleteImage(int imageId)
        {
             DeleteImageCommand request = new DeleteImageCommand { ImageId = imageId };
            bool result = await _mediator.Send(request);
            return NoContent();
        }

        [HttpGet("event/{eventId}")]
        public async Task<ActionResult<List<Media>>> GetImagesOfEvent(int eventId)
        {
             GetImagesOfEventQuery request = new GetImagesOfEventQuery { EventId = eventId };
            List<Media> result = await _mediator.Send(request);
            return Ok(result);
        }
    }
}

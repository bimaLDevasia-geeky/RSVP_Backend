using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RSVP.Application.Features.User.Command.CreateUser;

namespace RSVP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost()]

        public async Task<IActionResult> AddUser(CreateUserCommand command)
        {
            var userId = await _mediator.Send(command);
            return Ok(userId);
        }

    }
}

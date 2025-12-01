using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RSVP.Application.Dtos;
using RSVP.Application.Features.User.Command.CreateUser;
using RSVP.Application.Features.User.Command.UpdateUser;
using RSVP.Application.Features.User.Queries.GetUserByName;
using RSVP.Application.Features.User.Query;

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

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
          GetUsersQuery query = new GetUsersQuery();
          return Ok(await _mediator.Send(query));
           
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            GetUserByIdQuery query = new GetUserByIdQuery
            {
                UserId = id
            };
            UserDataDto userDto = await _mediator.Send(query);
            return Ok(userDto);
        }




        [HttpPost]

        public async Task<IActionResult> AddUser(CreateUserCommand command)
        {
            var userId = await _mediator.Send(command);
            return Ok(userId);
        }


        [HttpPatch("{id}")]
    
        public async Task<IActionResult> UpdateUser(UpdateUserCommand command, int id)
        {
                command.UserId = id;
                await _mediator.Send(command);
                return NoContent();
        }
        [HttpGet("search")]
        public async Task<IActionResult> GetUserByName([FromQuery] string term)
        {
            GetUserByNameQuery query = new GetUserByNameQuery
            {
                Term = term
            };
            List<RSVP.Domain.Entities.User> users = await _mediator.Send(query);
            return Ok(users);
        }

    }
}

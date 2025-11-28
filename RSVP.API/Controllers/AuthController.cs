using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RSVP.Application.Dtos;
using RSVP.Application.Features.Auth.Commands.Login;
using RSVP.Application.Features.Auth.Commands.Logout;
using RSVP.Application.Features.Auth.Commands.RefreshToken;

namespace RSVP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost("login")]
        public async Task<ActionResult<LoginResultDto>> Login(LoginCommand request)
        {
            LoginResultDto result = await _mediator.Send(request);
            return Ok(result);
        }
       
        [HttpPost("refresh")]
        public async Task<ActionResult> RefreshToken(RefreshAccessTokenCommand request)
        {
            
            RefreshDTO result = await _mediator.Send(request);
            return Ok(result);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult<bool>> Logout(LogoutCommand request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }
    }
}

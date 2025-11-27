using MediatR;
using RSVP.Application.Dtos;

namespace RSVP.Application.Features.Auth.Commands.Login;

public record class LoginCommand:IRequest<LoginResultDto>
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}

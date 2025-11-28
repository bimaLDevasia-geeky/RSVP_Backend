using MediatR;

namespace RSVP.Application.Features.Auth.Commands.Logout;

public class LogoutCommand : IRequest<bool>
{
}
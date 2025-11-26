using System;
using MediatR;

namespace RSVP.Application.Features.User.Command.CreateUser;

public class CreateUserCommand:IRequest<int>
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

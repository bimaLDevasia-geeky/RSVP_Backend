using System;
using MediatR;

namespace RSVP.Application.Features.User.Command.CreateUser;

public class CreateUserCommand:IRequest<int>
{
    public string Name { get; set; } =null!;
    public string Email { get; set; } =null!;
    public string Password { get; set; } =null!;
}

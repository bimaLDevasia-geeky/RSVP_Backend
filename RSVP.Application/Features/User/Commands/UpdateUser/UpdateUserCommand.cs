using System;
using System.Text.Json.Serialization;
using MediatR;
using RSVP.Domain.Enums;

namespace RSVP.Application.Features.User.Command.UpdateUser;

public class UpdateUserCommand:IRequest<bool>
{
   [JsonIgnore]
   public int UserId { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }

    public UserStatus? Status { get; set; }
    public UserRole? Role { get; set; } 
}

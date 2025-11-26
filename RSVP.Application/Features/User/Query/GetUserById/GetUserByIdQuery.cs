using System;
using System.Text.Json.Serialization;
using MediatR;
using RSVP.Application.Dtos;

namespace RSVP.Application.Features.User.Query;

public class GetUserByIdQuery:IRequest<UserDto>
{
    [JsonIgnore]
    public int UserId { get; set; }
}

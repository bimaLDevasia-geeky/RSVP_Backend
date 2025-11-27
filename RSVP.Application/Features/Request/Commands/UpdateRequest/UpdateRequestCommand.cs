using System;
using System.Text.Json.Serialization;
using MediatR;
using RSVP.Domain.Enums;

namespace RSVP.Application.Features.Request.Commands.UpdateRequest;

public class UpdateRequestCommand:IRequest<bool>
{
    [JsonIgnore]
    public int Id { get; set; }
    public RequestStatus Status { get; set; }
}

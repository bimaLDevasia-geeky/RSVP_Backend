using System;
using MediatR;
using RSVP.Domain.Enums;

namespace RSVP.Application.Features.Event.Commands.UpdateEventStatus;

public class UpdateEventStatusCommand:IRequest<bool>
{
    public int EventId { get; set; }
    public EventStatus Status { get; set; }
}

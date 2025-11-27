using System;
using MediatR;

namespace RSVP.Application.Features.Attendie.Command.AddAttendie;

public class AddAttendieCommand:IRequest<int>
{
    public int EventId { get; set; }
    public int UserId { get; set; }
}

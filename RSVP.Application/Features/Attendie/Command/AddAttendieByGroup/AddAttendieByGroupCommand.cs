using System;
using MediatR;

namespace RSVP.Application.Features.Attendie.Command.AddAttendieByGroup;

public class AddAttendieByGroupCommand:IRequest<bool>
{
    public int EventId { get; set; }
    public List<string> AttendieEmails { get; set; } = new List<string>();
}

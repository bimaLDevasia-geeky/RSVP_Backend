using System;
using MediatR;

namespace RSVP.Application.Features.Invite.Commands;

public class AcceptInviteCommand:IRequest<bool>
{
    public string InviteCode { get; set; }=null!;
    public Domain.Enums.AttendiesStatus Status { get; set; }
}

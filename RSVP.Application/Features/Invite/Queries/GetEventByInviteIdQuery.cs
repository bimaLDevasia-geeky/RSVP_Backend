using System;
using MediatR;
using RSVP.Application.Dtos;

namespace RSVP.Application.Features.Invite.Queries;

public class GetEventByInviteIdQuery:IRequest<EventDto>
{
    public string InviteCode { get; set; }=null!;
}

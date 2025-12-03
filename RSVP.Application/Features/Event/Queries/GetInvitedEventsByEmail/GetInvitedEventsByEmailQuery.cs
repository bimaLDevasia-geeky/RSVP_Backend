using System;
using MediatR;
using RSVP.Application.Dtos;

namespace RSVP.Application.Features.Event.Queries.GetInvitedEventsByEmail;

public class GetInvitedEventsByEmailQuery:IRequest<List<AttendieDto>>
{

}

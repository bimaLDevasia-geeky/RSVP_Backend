using System;
using MediatR;
using appDomain = RSVP.Domain.Entities;
namespace RSVP.Application.Features.Event.Queries.GetInvitedEvents;

public class GetInvitedEventQuery:IRequest<List<appDomain.Event>>
{

}

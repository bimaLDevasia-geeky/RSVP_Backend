using System;
using MediatR;
using appDomain = RSVP.Domain.Entities;
namespace RSVP.Application.Features.Event.Queries.GetEventOrgOrOwn;

public class GetEventOrgOrOwnQuery:IRequest<List<appDomain.Event>>
{

}

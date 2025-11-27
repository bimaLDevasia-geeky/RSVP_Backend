using System;
using MediatR;
using appDomain = RSVP.Domain.Entities;

namespace RSVP.Application.Features.Event.Queries.GetEventById;

public class GetEventByIdQuery:IRequest<appDomain.Event>
{
    public int Id { get; set; }
}

using System;
using MediatR;
using RSVP.Application.Dtos;
using appDomain = RSVP.Domain.Entities;
namespace RSVP.Application.Features.Event.Queries;

public class GetAllEventQuery:IRequest<List<EventDto>>
{
    
}

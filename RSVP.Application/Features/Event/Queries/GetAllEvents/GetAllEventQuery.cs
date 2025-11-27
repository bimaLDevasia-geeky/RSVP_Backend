using System;
using MediatR;
using appDomain = RSVP.Domain.Entities;
namespace RSVP.Application.Features.Event.Queries;

public class GetAllEventQuery:IRequest<List<appDomain.Event>>
{
    
}

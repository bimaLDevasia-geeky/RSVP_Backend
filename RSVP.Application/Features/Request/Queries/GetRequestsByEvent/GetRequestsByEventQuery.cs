using System;
using MediatR;
using RSVP.Application.Dtos;

namespace RSVP.Application.Features.Request.Queries.GetRequestsByEvent;

public class GetRequestsByEventQuery:IRequest<List<RequestDto>>
{
    public int EventId { get; set; }
}

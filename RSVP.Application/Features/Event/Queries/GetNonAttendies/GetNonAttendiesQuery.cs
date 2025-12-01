using System;
using MediatR;
using RSVP.Application.Dtos;

namespace RSVP.Application.Features.Event.Queries.GetNonAttendies;

public class GetNonAttendiesQuery:IRequest<List<UserDto>>
{

public int EventId { get; set; }
public string Term { get; set; }=null!;

}

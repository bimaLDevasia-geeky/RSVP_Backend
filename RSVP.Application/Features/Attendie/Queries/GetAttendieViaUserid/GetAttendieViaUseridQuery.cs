using System;
using MediatR;
using appDomain = RSVP.Domain.Entities;
namespace RSVP.Application.Features.Attendie.Queries.GetAttendieViaUserid;

public class GetAttendieViaUseridQuery:IRequest<appDomain.Attendie?>
{
    public int UserId { get; set; }
    public int EventId { get; set; }
}

using System;
using System.Text.Json.Serialization;
using MediatR;
using RSVP.Application.Dtos;
using RSVP.Domain.Enums;

namespace RSVP.Application.Features.Attendie.Queries.GetAttendiesViaEventFIlter;

public class GetAttendiesViaEventFIlterQuery:IRequest<GetAttendiesViaEventFilterResponseDto>
{   
    
     public int EventId { get; set; }
    public AttendiesStatus Status { get; set; }
}

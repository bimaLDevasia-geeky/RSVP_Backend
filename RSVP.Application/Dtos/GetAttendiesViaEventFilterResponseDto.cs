using System;

namespace RSVP.Application.Dtos;

public class GetAttendiesViaEventFilterResponseDto
{
    public List<Domain.Entities.Attendie> Attendies { get; set; } = new();
    public StatusCountDto StatusCounts { get; set; } = new();   
}

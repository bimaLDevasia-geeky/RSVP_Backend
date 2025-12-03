using System;

namespace RSVP.Application.Dtos;

public class AttendieDto
{

public int AttendieId { get; set; }
    public int EventId { get; set; }
    public string EventName { get; set; } = string.Empty;

    public DateTime EventDate { get; set; }

    public TimeOnly EventTime { get; set; }
    public string Status { get; set; } = string.Empty;
}

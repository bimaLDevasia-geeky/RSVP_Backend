using System;
using RSVP.Domain.Enums;

namespace RSVP.Application.Dtos;

public class EventDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime Date { get; set; }
    public string Venue { get; set; } = null!;
    public TimeOnly Time { get; set; }
    public bool IsPublic { get; set; }
    public EventStatus Status { get; set; }
    public AttendiesRole? MyRole { get; set; }
    public AttendiesStatus? MyResponseStatus { get; set; }
}

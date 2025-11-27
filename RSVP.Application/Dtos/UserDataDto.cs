using System;
using RSVP.Domain.Entities;
using RSVP.Domain.Enums;

namespace RSVP.Application.Dtos;

public class UserDataDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }

    public List<EventDto> CreatedEvents { get; set; } = new();

    public List<EventDto> InvitedEvents { get; set; } = new();

    public List<EventDto> OrganizedEvents { get; set; } = new();

    public List<NotificationDto> Notifications { get; set; } = new();

}


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

public class NotificationDto
{
    public int Id { get; set; }
    public string Description { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public string Route { get; set; } = null!;
}


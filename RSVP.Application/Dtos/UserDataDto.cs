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







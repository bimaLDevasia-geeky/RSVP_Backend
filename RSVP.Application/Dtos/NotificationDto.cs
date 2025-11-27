using System;

namespace RSVP.Application.Dtos;

public class NotificationDto
{
    public int Id { get; set; }
    public string Description { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public string Route { get; set; } = null!;
}

using System;

namespace RSVP.Domain.Entities;

public class Notification
{
    public int Id { get; private set; }
    public int UserId { get; private set; }
    public string Description { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public string Route { get; private set; }   


    public Notification(int userId, string description, string route)
    {
        UserId = userId;
        Description = description;
        Route = route;
        CreatedAt = DateTime.UtcNow;
    }

}

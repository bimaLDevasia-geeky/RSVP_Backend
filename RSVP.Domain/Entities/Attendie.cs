using System;
using RSVP.Domain.Enums;

namespace RSVP.Domain.Entities;

public class Attendie
{
    public int Id { get; private set; }
    public int EventId { get; private set; }
    public int? UserId { get; private set; }
    public string Email { get; private set; }=null!;

    public AttendiesRole Role { get; private set; } = AttendiesRole.Guest;
    public AttendiesStatus Status { get; private set; } = AttendiesStatus.NoResponse; 

    public Event Event { get; private set; } = null!;
    public User? User { get; private set; }

    

    public Attendie(int eventId, int? userId, string email)
    {
        EventId = eventId;
        if(userId.HasValue)
        UserId = userId.Value;
        Email = email;
    }
    public void UpdateUserId(int userId)
    {
        UserId = userId;
    }
    public void UpdateRole(AttendiesRole role)
    {
        Role = role;
    }
    public void UpdateStatus(AttendiesStatus status)
    {
        Status = status;
    }
}

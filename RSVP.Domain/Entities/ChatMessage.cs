using System;

namespace RSVP.Domain.Entities;

public class ChatMessage
{
     public int Id { get; set; }
    public int EventId { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; } = null!;
    public string Message { get; set; } = null!;
    public DateTime Timestamp { get; set; }
    public Event Event { get; set; } = null!;
    public User User { get; set; } = null!;

    

    public ChatMessage(int eventId, int userId, string userName, string message)
    {
        EventId = eventId;
        UserId = userId;
        UserName = userName;
        Message = message;
        Timestamp = DateTime.UtcNow;
    }
}

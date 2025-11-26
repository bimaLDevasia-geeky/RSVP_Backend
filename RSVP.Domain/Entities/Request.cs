using System;
using RSVP.Domain.Enums;

namespace RSVP.Domain.Entities;

public class Request
{
    public int Id { get; private set; }
    public int EventId { get; private set; }
    public int UserId { get; private set; }
    public DateTime RequestedAt { get; private set; }
    public RequestStatus Status { get; private  set; } = RequestStatus.Pending;
    
    public Request(int eventId, int  userId)
    {
        EventId = eventId;
        UserId = userId;
        RequestedAt = DateTime.UtcNow;
    }

    public  void UpdateStatus(RequestStatus newStatus)
    {
        Status = newStatus;
    }


}

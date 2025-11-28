using System;

namespace RSVP.Domain.Entities;

public class Media
{
    public int Id {get; private set; }
    public int EventId {get; private set; }
    public string Url {get; private set; }=null!;
    public string PublicId { get; private set; } = null!;
    public Event Event { get; private set; } = null!;

    public Media(int eventId, string url, string publicId)
    {
        EventId = eventId;
        Url = url;
        PublicId = publicId;
    }

    public void UpdateUrl(string url)
    {
        Url = url;
    }

}


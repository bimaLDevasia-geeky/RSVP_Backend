using System;

namespace RSVP.Domain.Entities;

public class Media
{
    public int Id {get; private set; }
    public int EventId {get; private set; }
    public string Url {get; private set; }=null!;

    public Media(int eventId, string url)
    {
        EventId = eventId;
        Url = url;
    }

    public void UpdateUrl(string url)
    {
        Url = url;
    }

}


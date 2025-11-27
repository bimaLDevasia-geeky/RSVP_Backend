using System;

namespace RSVP.Application.Dtos;

public class StatusCountDto
{
    public int Attending { get; set; }
    public int NotAttending { get; set; }
    public int Maybe { get; set; }
    public int NoResponse { get; set; }
}

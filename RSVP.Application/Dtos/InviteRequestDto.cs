using System;

namespace RSVP.Application.Dtos;

public class InviteRequestDto
{

public int UserId { get; set; }

public int EventId { get; set; }
public string EventName { get; set; } = null!;


}

using System;

namespace RSVP.Application.Dtos;

public class ChatMessageDTO
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; }=null!;
    public string Message { get; set; } = null!;

}

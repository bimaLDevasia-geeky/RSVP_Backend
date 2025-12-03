using System;
using MediatR;
using RSVP.Application.Dtos;

namespace RSVP.Application.Features.Chat.Command;

public class AddMessageCommand:IRequest<ChatMessageDTO>
{
    public int EventId { get; set; }
    public string Message { get; set; } = null!;
}

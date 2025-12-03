using System;
using MediatR;
using RSVP.Application.Dtos;

namespace RSVP.Application.Features.Chat.Queries;

public class GetMessageQuery:IRequest<List<ChatMessageDTO>>
{
    public int EventId { get; set; }
}

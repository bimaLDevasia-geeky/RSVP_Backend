using System;
using MediatR;
using RSVP.Application.Dtos;
using RSVP.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace RSVP.Application.Features.Chat.Queries;

public class GetMessageQueryHandler:IRequestHandler<GetMessageQuery,List<ChatMessageDTO>>
{
    private readonly IRsvpDbContext _context;
    public GetMessageQueryHandler(IRsvpDbContext context)
    {
        _context = context;
    }
    public async Task<List<ChatMessageDTO>> Handle(GetMessageQuery request, CancellationToken cancellationToken)
    {
        var messages = _context.ChatMessages
            .Where(cm => cm.EventId == request.EventId)
            .Select(cm => new ChatMessageDTO
            {
                Id = cm.Id,
                EventId = cm.EventId,
                UserId = cm.UserId,
                UserName = cm.UserName,
                Message = cm.Message
            });

        return await messages.ToListAsync(cancellationToken);
    }
}

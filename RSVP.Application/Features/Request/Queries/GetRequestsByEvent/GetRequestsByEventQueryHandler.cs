using System;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RSVP.Application.Dtos;
using RSVP.Application.Interfaces;

namespace RSVP.Application.Features.Request.Queries.GetRequestsByEvent;

public class GetRequestsByEventQueryHandler:IRequestHandler<GetRequestsByEventQuery, List<RequestDto>>
{
    private readonly IRsvpDbContext _context;
    private readonly IRepository<Domain.Entities.User> _userRepository;

    private readonly IRepository<Domain.Entities.Event> _eventRepository;

    public GetRequestsByEventQueryHandler(IRsvpDbContext context, IRepository<Domain.Entities.User> userRepository, IRepository<Domain.Entities.Event> eventRepository)
    {
        _context = context;
        _userRepository = userRepository;
        _eventRepository = eventRepository;
    }

    public async Task<List<RequestDto>> Handle(GetRequestsByEventQuery request, CancellationToken cancellationToken)
    {

        string userName = (await _userRepository.GetByIdAsync(request.EventId, cancellationToken))?.Name ?? "Unknown User";
        string eventName = (await _eventRepository.GetByIdAsync(request.EventId, cancellationToken))?.Name ?? "Unknown Event";
        var requests = await _context.Requests
            .Where(r => r.EventId == request.EventId && r.Status == Domain.Enums.RequestStatus.Pending)
            .Select(r => new RequestDto(
                r.Id,
                r.EventId,
                r.UserId,
                r.RequestedAt,
                r.Status,
                userName,
                eventName
            ))
            .ToListAsync(cancellationToken);

        return requests;
        
        
    }

}

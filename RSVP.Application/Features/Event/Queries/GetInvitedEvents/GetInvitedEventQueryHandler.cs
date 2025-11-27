using System;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RSVP.Application.Interfaces;
using appDomain = RSVP.Domain.Entities;
namespace RSVP.Application.Features.Event.Queries.GetInvitedEvents;

public class GetInvitedEventQueryHandler:IRequestHandler<GetInvitedEventQuery,List<appDomain.Event>>
{
    private readonly IRsvpDbContext _context;
    private readonly ICurrentUser _currentUserService;
    public GetInvitedEventQueryHandler(IRsvpDbContext context, ICurrentUser currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<List<appDomain.Event>> Handle(GetInvitedEventQuery request, CancellationToken cancellationToken)
    {
        int userId = _currentUserService.UserId;
        List<appDomain.Event> events = await _context.Events
                                            .AsNoTracking()
                                            .Where(e =>e.CreatedBy != userId && e.Attendies.Any(a => a.UserId == userId && (a.Role != Domain.Enums.AttendiesRole.Organizer)))
                                            .ToListAsync(cancellationToken);
        return events;
    }
}

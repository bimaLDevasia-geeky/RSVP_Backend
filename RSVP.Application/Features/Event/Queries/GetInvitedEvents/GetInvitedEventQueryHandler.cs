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
        string emailId = _currentUserService.Email;
        List<appDomain.Event> events = await _context.Events
                                            .Include(e=>e.Media)
                                            .AsNoTracking()
                                            .Where(e =>e.CreatedBy != userId && e.Attendies.Any(a => a.Email == emailId && (a.Role != Domain.Enums.AttendiesRole.Organizer)) && !(e.Status == Domain.Enums.EventStatus.Completed))
                                            .ToListAsync(cancellationToken);
        return events;
    }
}

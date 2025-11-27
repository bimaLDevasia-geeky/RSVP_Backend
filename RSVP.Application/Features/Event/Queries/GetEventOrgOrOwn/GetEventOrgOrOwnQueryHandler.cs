using System;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RSVP.Application.Interfaces;
using RSVP.Domain.Enums;
using appDomain = RSVP.Domain.Entities;
namespace RSVP.Application.Features.Event.Queries.GetEventOrgOrOwn;

public class GetEventOrgOrOwnQueryHandler:IRequestHandler<GetEventOrgOrOwnQuery,List<appDomain.Event>>
{   
    private readonly IRsvpDbContext _context;
    private readonly ICurrentUser _currentUserService;
    public GetEventOrgOrOwnQueryHandler(IRsvpDbContext context, ICurrentUser currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }
    public async Task<List<appDomain.Event>> Handle(GetEventOrgOrOwnQuery request, CancellationToken cancellationToken)
    {
        int userId = _currentUserService.UserId;
        List<appDomain.Event> events = await _context.Events
                                            .Include(e => e.Attendies)
                                            .AsNoTracking()
                                            .Where(e => e.CreatedBy == userId || e.Attendies.Any(a => a.UserId == userId&& a.Role ==AttendiesRole.Organizer))
                                            .ToListAsync();
        return events;
    }
}

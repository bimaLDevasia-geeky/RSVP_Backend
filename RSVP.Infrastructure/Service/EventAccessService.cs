using System;
using Microsoft.EntityFrameworkCore;
using RSVP.Application.Interfaces;
using RSVP.Domain.Entities;
using RSVP.Domain.Enums;
using RSVP.Infrastructure.Persistence;

namespace RSVP.Infrastructure.Service;

public class EventAccessService: IEventAccessService
{
        private readonly RsvpDbContext _context;
        private readonly ICurrentUser _currentUser;
        public EventAccessService(RsvpDbContext context, ICurrentUser currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }
    public async Task<bool> IsOrganizerOrOwnerAsync(int eventId, CancellationToken ct)
    {
        
        var userId = _currentUser.UserId;

    
    bool hasPermission = await _context.Events
        .AsNoTracking()
        .AnyAsync(e => 
            e.Id == eventId &&
            (
                e.CreatedBy == userId || 
                e.Attendies.Any(a => a.UserId == userId && a.Role == AttendiesRole.Organizer) 
            ), ct);

    return hasPermission;
    }
}

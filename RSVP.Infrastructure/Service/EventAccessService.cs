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
        
        Attendie? attendie = await _context.Attendies
            .FirstOrDefaultAsync(a => a.UserId == _currentUser.UserId && a.EventId == eventId && (a.Role == AttendiesRole.Organizer || a.Role == AttendiesRole.Owner), ct);
        
            if (attendie is null)
            {
                return false;
            }
            return true;
    }
}

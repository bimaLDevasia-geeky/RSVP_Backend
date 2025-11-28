using System;
using Microsoft.EntityFrameworkCore;
using RSVP.Application.Interfaces;
using RSVP.Domain.Entities;
using RSVP.Infrastructure.Persistence;

namespace RSVP.Infrastructure.Repositories;

public class EventRepository:GenericRepository<Event>, IEventRepository
{
    public EventRepository(RsvpDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Event?> GetEventByInviteCodeAsync(string inviteCode, CancellationToken cancellationToken)
    {
        return await _context.Events
            .FirstOrDefaultAsync(e => e.InviteCode == inviteCode, cancellationToken);
    }
}

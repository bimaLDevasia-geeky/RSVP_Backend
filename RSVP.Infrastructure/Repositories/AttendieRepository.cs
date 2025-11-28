using System;
using Microsoft.EntityFrameworkCore;
using RSVP.Application.Interfaces;
using RSVP.Domain.Entities;
using RSVP.Infrastructure.Persistence;

namespace RSVP.Infrastructure.Repositories;

public class AttendieRepository:GenericRepository<Attendie>, IAttendieRepository
{
    public AttendieRepository(RsvpDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Attendie?> GetAttendieByEmailAndEventIdAsync(string email, int eventId, CancellationToken cancellationToken)
    {
        return await _context.Attendies
            .FirstOrDefaultAsync(a => a.Email == email && a.EventId == eventId, cancellationToken);
    }
}


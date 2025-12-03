using System;
using Microsoft.EntityFrameworkCore;
using RSVP.Application.Interfaces;
using RSVP.Domain.Entities;
using RSVP.Domain.Enums;

namespace RSVP.Infrastructure.Service;

public class UpdateEventDaily
{
    private readonly IRsvpDbContext _context;
    public UpdateEventDaily(IRsvpDbContext context)
    {
        _context = context;
    }
    public async Task UpdateEventStatusDaily()
    {
        var today = DateTime.UtcNow.Date;

        

       await _context.Events
        .Where(e => e.Date < today && e.Status == EventStatus.Active)
        .ExecuteUpdateAsync(setters => setters
            .SetProperty(e => e.Status, EventStatus.Completed));

        
    }
}

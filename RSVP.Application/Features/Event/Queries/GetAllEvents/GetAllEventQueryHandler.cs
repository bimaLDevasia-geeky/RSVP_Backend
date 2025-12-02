using System;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RSVP.Application.Interfaces;
using appDomain = RSVP.Domain.Entities;

namespace RSVP.Application.Features.Event.Queries.GetAllEvents;

public class GetAllEventQueryHandler:IRequestHandler<GetAllEventQuery, List<appDomain.Event>>
{
    private readonly IRsvpDbContext _context;

    public GetAllEventQueryHandler(IRsvpDbContext context)
    {
        _context = context;
    }

    public async Task<List<appDomain.Event>> Handle(GetAllEventQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<appDomain.Event> events = await _context.Events
                                                    .Include(e=>e.Media)
                                                    .AsNoTracking()
                                                    .ToListAsync(cancellationToken);
        return events.ToList();
    }
}

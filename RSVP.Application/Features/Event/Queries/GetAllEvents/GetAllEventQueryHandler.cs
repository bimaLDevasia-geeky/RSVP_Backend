using System;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RSVP.Application.Dtos;
using RSVP.Application.Interfaces;
using appDomain = RSVP.Domain.Entities;

namespace RSVP.Application.Features.Event.Queries.GetAllEvents;

public class GetAllEventQueryHandler:IRequestHandler<GetAllEventQuery, List<EventDto>>
{
    private readonly IRsvpDbContext _context;

    public GetAllEventQueryHandler(IRsvpDbContext context)
    {
        _context = context;
    }

    public async Task<List<EventDto>> Handle(GetAllEventQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<appDomain.Event> events = await _context.Events
                                                    .Include(e=>e.Media)
                                                    .AsNoTracking()
                                                    .ToListAsync(cancellationToken);
                                                    
        return events.Select(e => new EventDto
        {
            Id = e.Id,
            Name = e.Name,
            Description = e.Description,
            Date = e.Date,
            Time = e.Time,
            Venue = e.Venue,
            IsPublic = e.IsPublic,
            CreatedBy = e.CreatedBy,
            Status = e.Status,
            Media = e.Media.Select(m => new MediaDto
            (
                m.Id,
                m.Url
            )).ToList()
        }).ToList();
    }
}

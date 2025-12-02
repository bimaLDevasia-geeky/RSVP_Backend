using System;
using RSVP.Application.Interfaces;
using MediatR;
using appDomain = RSVP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace RSVP.Application.Features.Event.Queries.GetEventById;

public class GetEventByIdQueryHandler: IRequestHandler<GetEventByIdQuery, appDomain.Event>

{
    private readonly IRsvpDbContext _context;
    public GetEventByIdQueryHandler(IRsvpDbContext context)
    {
        _context = context;
    }

    public async Task<appDomain.Event> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
    {
        appDomain.Event? evnt = await _context.Events
                                            .Include(e=>e.Media)
                                            .Include(e=>e.Attendies)
                                            .AsNoTracking()
                                            .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
        if (evnt is null)
        {
            throw new KeyNotFoundException($"Event with Id {request.Id} not found.");
        }
        return evnt;
    }
}

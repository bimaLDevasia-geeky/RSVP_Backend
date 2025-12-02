using System;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RSVP.Application.Interfaces;
using appDomain = RSVP.Domain.Entities;
namespace RSVP.Application.Features.Attendie.Queries.GetAttendieViaUserid;

public class GetAttendieViaUseridQueryHandler:IRequestHandler<GetAttendieViaUseridQuery, appDomain.Attendie?>
{
    private readonly IRsvpDbContext _context;
    public GetAttendieViaUseridQueryHandler(IRsvpDbContext context)
    {
        _context = context;
    }

    public async Task<appDomain.Attendie?> Handle(GetAttendieViaUseridQuery request, CancellationToken cancellationToken)
    {
        appDomain.Attendie? attendie = await _context.Attendies
                                            .AsNoTracking()
                                            .FirstOrDefaultAsync(a => a.UserId == request.UserId && a.EventId == request.EventId, cancellationToken);
        return attendie;
    }
}

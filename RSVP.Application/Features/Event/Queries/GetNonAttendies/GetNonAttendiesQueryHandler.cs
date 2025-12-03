using System;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RSVP.Application.Dtos;
using RSVP.Application.Interfaces;
using RSVP.Domain.Enums;
using appDOmain = RSVP.Domain.Entities; 
namespace RSVP.Application.Features.Event.Queries.GetNonAttendies;

public class GetNonAttendiesQueryHandler : IRequestHandler<GetNonAttendiesQuery, List<UserDto>>
{

    private readonly IRsvpDbContext _context;
    private readonly IEventAccessService _eventAccessService;

    public GetNonAttendiesQueryHandler(IRsvpDbContext context, IEventAccessService eventAccessService)
    {
        _context = context;
        _eventAccessService = eventAccessService;
    }

    public async Task<List<UserDto>> Handle(GetNonAttendiesQuery request, CancellationToken cancellationToken)
    {
        if ( !await _eventAccessService.IsOrganizerOrOwnerAsync(request.EventId, cancellationToken))
        {
            throw new UnauthorizedAccessException("You do not have permission to access this event's non-attendies.");
        }

       var attendeeIdsQuery = _context.Attendies
                    .AsNoTracking()
                .Where(a => a.EventId == request.EventId)
                .Select(a => a.UserId);
        appDOmain.Event? evnt = await _context.Events
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(e => e.Id == request.EventId, cancellationToken);

        var nonAttendies = await _context.Users
                .AsNoTracking()
                .Where(u => u.Role != UserRole.Admin && !attendeeIdsQuery.Contains(u.Id) && u.Id != evnt!.CreatedBy && u.Name.Contains(request.Term))
                .Select(u => new UserDto(u.Id, u.Name, u.Email, u.Status))
                .ToListAsync(cancellationToken);

        return nonAttendies;
    }
}

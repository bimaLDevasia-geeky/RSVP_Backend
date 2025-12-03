using System;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RSVP.Application.Dtos;
using RSVP.Application.Interfaces;

namespace RSVP.Application.Features.Event.Queries.GetInvitedEventsByEmail;

public class GetInvitedEventsByEmailQueryHandler:IRequestHandler<GetInvitedEventsByEmailQuery, List<AttendieDto>>
{


    private readonly IRsvpDbContext _context;
    private readonly ICurrentUser _currentUser;

    public GetInvitedEventsByEmailQueryHandler(IRsvpDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<List<AttendieDto>> Handle(GetInvitedEventsByEmailQuery request, CancellationToken cancellationToken)
    {
        var emailId = _currentUser.Email;

        var attendie = await _context.Attendies
           .Include(a => a.Event)
           .Where(a => a.Email == emailId && (a.Role != Domain.Enums.AttendiesRole.Organizer) && !(a.Event.Status == Domain.Enums.EventStatus.Completed))
              .Select(a => new AttendieDto
              {
                AttendieId = a.Id,
                EventId = a.EventId,
                EventName = a.Event.Name,
                EventDate = a.Event.Date,
                EventTime = a.Event.Time,
                Status = a.Status.ToString()
              }).ToListAsync(cancellationToken);
            
           return attendie;

        
    }
}

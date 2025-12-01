using System;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RSVP.Application.Dtos;
using RSVP.Application.Interfaces;
using RSVP.Domain.Enums;

namespace RSVP.Application.Features.Attendie.Queries.GetAttendiesViaEventFIlter;

public class GetAttendiesViaEventFIlterCommandHandler : IRequestHandler<GetAttendiesViaEventFIlterQuery, GetAttendiesViaEventFilterResponseDto>
{
    private readonly IRsvpDbContext _context;

    public GetAttendiesViaEventFIlterCommandHandler(IRsvpDbContext context)
    {
        _context = context;
    }

    public async Task<GetAttendiesViaEventFilterResponseDto> Handle(GetAttendiesViaEventFIlterQuery request, CancellationToken cancellationToken)
    {
       
        var allAttendies = await _context.Attendies
            .Where(a => a.EventId == request.EventId)
            .Include(a => a.User)
            .Include(a => a.Event)
            .ToListAsync(cancellationToken);

      
        
            
      
        var statusCounts = new StatusCountDto
        {
            Attending = allAttendies.Count(a => a.Status == AttendiesStatus.Attending),
            NotAttending = allAttendies.Count(a => a.Status == AttendiesStatus.NotAttending),
            Maybe = allAttendies.Count(a => a.Status == AttendiesStatus.Maybe),
            NoResponse = allAttendies.Count(a => a.Status == AttendiesStatus.NoResponse)
        };

        return new GetAttendiesViaEventFilterResponseDto
        {
            Attendies = allAttendies,
            StatusCounts = statusCounts
        };
    }
}

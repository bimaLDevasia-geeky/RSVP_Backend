using System;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RSVP.Application.Dtos;
using RSVP.Application.Interfaces;
using RSVP.Domain.Enums;

namespace RSVP.Application.Features.User.Query.GetUserById;

public class GetUserByIdQueryHandler:IRequestHandler<GetUserByIdQuery,UserDataDto>
{

   private readonly IRsvpDbContext _context;

    public GetUserByIdQueryHandler(IRsvpDbContext context)
    {
        _context = context;
    }
    
public async Task<UserDataDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
{
    var userId = request.UserId;

    var user = await _context.Users
        .AsNoTracking()
        .Include(u => u.CreatedEvents)
        .Include(u => u.Notifications)
        .Include(u => u.Attendies).ThenInclude(a => a.Event)
        .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

    if (user == null) return null!;

    var notifications = user.Notifications
        .OrderByDescending(n => n.CreatedAt)
        .Select(n => new NotificationDto
        {
            Id = n.Id,
            Description = n.Description,
            CreatedAt = n.CreatedAt,
            Route = n.Route
        })
        .ToList();

    EventDto MapEvent(Domain.Entities.Event e, AttendiesRole? role = null, AttendiesStatus? status = null)
        => new()
        {
            Id = e.Id,
            Name = e.Name,
            Description = e.Description,
            Date = e.Date,
            Venue = e.Venue,
            Time = e.Time,
            IsPublic = e.IsPublic,
            Status = e.Status,
            MyRole = role,
            MyResponseStatus = status
        };

    var createdEvents = user.CreatedEvents
        .Select(e => MapEvent(e, AttendiesRole.Organizer))
        .ToList();

    var createdEventIds = createdEvents.Select(e => e.Id).ToHashSet();

    var organizerAttendies = user.Attendies
        .Where(a => a.Event != null && a.Role == AttendiesRole.Organizer)
        .ToList();

    var organizedEvents = organizerAttendies
        .Select(a => MapEvent(a.Event!, AttendiesRole.Organizer, a.Status))
        .Concat(createdEvents) 
        .DistinctBy(e => e.Id)
        .ToList();

    var invitedEvents = user.Attendies
        .Where(a => a.Event != null
                    && !createdEventIds.Contains(a.Event.Id)           
                    && a.Role != AttendiesRole.Organizer)          
        .Select(a => MapEvent(a.Event!, a.Role, a.Status))
        .ToList();

    return new UserDataDto
    {
        Id = user.Id,
        Name = user.Name,
        Email = user.Email,

        CreatedEvents = createdEvents,
        OrganizedEvents = organizedEvents,
        InvitedEvents = invitedEvents,  
        Notifications = notifications
    };
}
}

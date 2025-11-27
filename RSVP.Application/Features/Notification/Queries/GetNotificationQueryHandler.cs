using System;
using MediatR;
using RSVP.Application.Dtos;
using RSVP.Application.Interfaces;

namespace RSVP.Application.Features.Notification.Queries;

public class GetNotificationQueryHandler:IRequestHandler<GetNotificationQuery, List<NotificationDto>>
{

    private readonly IRsvpDbContext _context;
    private readonly ICurrentUser _currentUser;

    public GetNotificationQueryHandler(IRsvpDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public Task<List<NotificationDto>> Handle(GetNotificationQuery request, CancellationToken cancellationToken)
    {
        var notifications = _context.Notifications
            .Where(n => n.UserId == _currentUser.UserId)
            .Select(n => new NotificationDto
            {
                Id = n.Id,
                Description = n.Description,
                CreatedAt = n.CreatedAt,
                Route = n.Route
            })
            .ToList();

        return Task.FromResult(notifications);
    }
}

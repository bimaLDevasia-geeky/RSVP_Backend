using System;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RSVP.Application.Dtos;
using RSVP.Application.Interfaces;

namespace RSVP.Application.Features.User.Queries.GetInviteRequests;

public class GetInviteRequestsQueryHandler:IRequestHandler<GetInviteRequestsQuery, List<InviteRequestDto>>
{

    private readonly IRsvpDbContext _context;

    private readonly ICurrentUser _currentUser;

    public GetInviteRequestsQueryHandler(IRsvpDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<List<InviteRequestDto>> Handle(GetInviteRequestsQuery request, CancellationToken cancellationToken)
    {
        var inviteRequests = await _context.Attendies
            .AsNoTracking()
            .Where(a => a.Status == Domain.Enums.AttendiesStatus.NoResponse && a.UserId ==_currentUser.UserId)
            .Select(a => new InviteRequestDto
            {
                UserId = _currentUser.UserId,
                EventId = a.EventId,
                EventName = a.Event.Name,
            })
            .ToListAsync(cancellationToken);

        return inviteRequests;
    }
}

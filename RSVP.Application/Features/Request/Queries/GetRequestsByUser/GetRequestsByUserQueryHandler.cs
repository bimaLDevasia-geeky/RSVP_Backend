using System;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RSVP.Application.Dtos;
using RSVP.Application.Interfaces;

namespace RSVP.Application.Features.Request.Queries.GetRequestsByUser;

public class GetRequestsByUserQueryHandler:IRequestHandler<GetRequestsByUserQuery, List<RequestDto>>
{

    private readonly IRsvpDbContext _context;

    private readonly ICurrentUser _currentUser;

    public GetRequestsByUserQueryHandler(IRsvpDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public Task<List<RequestDto>> Handle(GetRequestsByUserQuery request, CancellationToken cancellationToken)
    {

        var requests =  _context.Requests
            .Where(r => r.UserId == _currentUser.UserId)
            .Select(r => new RequestDto(
                r.Id,
                r.EventId,
                r.UserId,
                r.RequestedAt,
                r.Status
            ))
            .ToListAsync(cancellationToken);

        return requests;
        
        
    }
}

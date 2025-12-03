using System;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RSVP.Application.Dtos;
using RSVP.Application.Interfaces;
using RSVP.Domain.Entities;

namespace RSVP.Application.Features.Invite.Queries;

public class GetEventByInviteIdQueryHandler:IRequestHandler<GetEventByInviteIdQuery, EventDto>
{

    private readonly IRsvpDbContext _context;

    private readonly ICurrentUser? _currentUser = null;


    private int attendieId = 0;

    public GetEventByInviteIdQueryHandler(IRsvpDbContext context, ICurrentUser? currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<EventDto> Handle(GetEventByInviteIdQuery request, CancellationToken cancellationToken)
    {

        if(_currentUser != null)
        {

            attendieId = await _context.Attendies
                    .AsNoTracking()
                    .Where(a => a.Event.InviteCode == request.InviteCode 
                     && a.Email == _currentUser.Email)
                    .Select(a => a.Id)
                    .FirstOrDefaultAsync(cancellationToken);
        }


        var result = await _context.Events
            .Include(e => e.Media)
           .Where(e => e.InviteCode == request.InviteCode)
           .Select(e => new EventDto
           {
               Id = e.Id,
               Name = e.Name,
               Description = e.Description,
               Date = e.Date,
               Time = e.Time,
               Venue = e.Venue,
               IsPublic = e.IsPublic,
                Status = e.Status,
                attendieId = attendieId,
               Media = e.Media.Select(m => new MediaDto(m.Id, m.Url)).ToList(),
               CreatorName = _context.Users.Where(u => u.Id == e.CreatedBy).Select(u => u.Name).FirstOrDefault()
           }).FirstOrDefaultAsync(cancellationToken);

        return result is null ? throw new KeyNotFoundException("Invite not found.") : result;
    }
}

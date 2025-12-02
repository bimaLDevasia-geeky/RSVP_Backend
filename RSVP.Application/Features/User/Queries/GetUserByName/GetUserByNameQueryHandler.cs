using System;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RSVP.Application.Interfaces;

namespace RSVP.Application.Features.User.Queries.GetUserByName;

public class GetUserByNameQueryHandler:IRequestHandler<GetUserByNameQuery, List<RSVP.Domain.Entities.User>>
{   
    private readonly IRsvpDbContext _context;
    public GetUserByNameQueryHandler(IRsvpDbContext context)
    {
        _context = context;
    }
    public async Task<List<RSVP.Domain.Entities.User>> Handle(GetUserByNameQuery request, CancellationToken cancellationToken)
    {
        var users = await _context.Users
                                 .Where(u => u.Name.Contains(request.Term))
                                 .ToListAsync(cancellationToken);
        return users;
    }
}

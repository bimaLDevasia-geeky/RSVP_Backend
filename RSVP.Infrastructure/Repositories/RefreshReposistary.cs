using System;
using Microsoft.EntityFrameworkCore;
using RSVP.Domain.Entities;
using RSVP.Infrastructure.Persistence;

namespace RSVP.Infrastructure.Repositories;

public class RefreshReposistary:GenericRepository<RefreshToken>
{
    public RefreshReposistary(RsvpDbContext context) : base(context)
    {
    }

    public async Task<RefreshToken?> GetByToken(string token)
    {
        return await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == token);
    }
}

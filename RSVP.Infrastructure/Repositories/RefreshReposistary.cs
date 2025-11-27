using System;
using Microsoft.EntityFrameworkCore;
using RSVP.Application.Interfaces;
using RSVP.Domain.Entities;
using RSVP.Infrastructure.Persistence;

namespace RSVP.Infrastructure.Repositories;

public class RefreshReposistary:GenericRepository<RefreshToken>,IRefreshReposistary
{
    public RefreshReposistary(RsvpDbContext context) : base(context)
    {
    }

    public async Task<RefreshToken?> GetByToken(string token)
    {
        return await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == token);
    }
}

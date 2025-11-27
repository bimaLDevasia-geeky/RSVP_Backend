using System;
using Microsoft.EntityFrameworkCore;
using RSVP.Application.Interfaces;
using RSVP.Domain.Entities;
using RSVP.Infrastructure.Persistence;

namespace RSVP.Infrastructure.Repositories;

public class UserRepository:GenericRepository<User>,IUserReposistory
{   
    
    public UserRepository(RsvpDbContext context) : base(context) 
    {
    }

    public async Task<User?> GetUserByEmailAsync(string email, CancellationToken ct)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email, ct);
    }
}

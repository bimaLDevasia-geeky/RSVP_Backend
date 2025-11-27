using System;
using RSVP.Domain.Entities;

namespace RSVP.Application.Interfaces;

public interface IUserReposistory:IRepository<User>
{
    Task<User?> GetUserByEmailAsync(string email,CancellationToken ct);
}

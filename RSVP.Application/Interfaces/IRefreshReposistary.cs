using System;
using RSVP.Domain.Entities;

namespace RSVP.Application.Interfaces;

public interface IRefreshReposistary:IRepository<RefreshToken>
{
     Task<RefreshToken?> GetByToken(string token);
}

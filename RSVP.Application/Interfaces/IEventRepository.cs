using System;
using RSVP.Domain.Entities;

namespace RSVP.Application.Interfaces;

public interface IEventRepository:IRepository<Event>
{
    Task<Event?> GetEventByInviteCodeAsync(string inviteCode, CancellationToken cancellationToken);
}

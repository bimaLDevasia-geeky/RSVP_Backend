using System;
using RSVP.Domain.Entities;

namespace RSVP.Application.Interfaces;

public interface IAttendieRepository:IRepository<Attendie>
{
    Task<Attendie?> GetAttendieByEmailAndEventIdAsync(string email, int eventId, CancellationToken cancellationToken);
}

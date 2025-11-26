using System;
using RSVP.Domain.Entities;

namespace RSVP.Application.Interfaces;

public interface  IEventAccessService
{
    Task<bool> IsOrganizerOrOwnerAsync(int eventId, CancellationToken ct);
}
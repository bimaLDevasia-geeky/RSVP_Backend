using System;
using RSVP.Domain.Entities;
using RSVP.Domain.Enums;

namespace RSVP.Application.Dtos;

public record RequestDto(
    int Id,
    int EventId,
    int UserId,
    DateTime RequestedAt,
    RequestStatus Status,

    string UserName,
    string EventName
);

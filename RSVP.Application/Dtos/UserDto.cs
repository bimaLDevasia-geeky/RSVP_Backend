using System;
using RSVP.Domain.Enums;

namespace RSVP.Application.Dtos;

public record UserDto(int Id, string Name, string Email,UserStatus Status);

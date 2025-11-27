using FluentValidation.Validators;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using RSVP.Domain.Enums;

namespace RSVP.Application.Dtos;

public record class LoginResultDto
(
     int id,
    string email,
    string token,
    UserRole role

);

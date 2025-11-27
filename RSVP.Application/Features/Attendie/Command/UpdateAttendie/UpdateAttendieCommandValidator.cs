using System;
using System.Data;
using FluentValidation;
using MediatR;

namespace RSVP.Application.Features.Attendie.Command.UpdateAttendie;

public class UpdateAttendieCommandValidator:AbstractValidator<UpdateAttendieCommand>
{
    public UpdateAttendieCommandValidator()
    {
        RuleFor(x => x)
            .Must(x => x.Role.HasValue || x.Status.HasValue)
            .WithMessage("At least one of Role or Status must be provided.");

        RuleFor(x =>x.AttendieId)
            .GreaterThan(0)
            .NotEmpty()
            .WithMessage("AttendieId must be a positive integer.");
    }
}

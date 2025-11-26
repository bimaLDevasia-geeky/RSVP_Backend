using System;
using FluentValidation;

namespace RSVP.Application.Features.Event.Commands.UpdateEvent;

public class UpdateEventCommandValidator:AbstractValidator<UpdateEventCommand>
{
    public UpdateEventCommandValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(100).WithMessage("Event name must not exceed 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Event description must not exceed 1000 characters.");

        RuleFor(x => x.Date)
            .GreaterThan(DateTime.Now).WithMessage("Event date must be in the future.");
            

        RuleFor(x => x.Venue)
            .MaximumLength(200).WithMessage("Event venue must not exceed 200 characters.");
    }
}

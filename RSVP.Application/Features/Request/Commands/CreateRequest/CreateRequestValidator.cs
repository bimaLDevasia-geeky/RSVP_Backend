using System;
using FluentValidation;

namespace RSVP.Application.Features.Request.Commands.CreateRequest;

public class CreateRequestValidator: AbstractValidator<CreateRequestCommand>
{
    public CreateRequestValidator()
    {
        RuleFor(x => x.EventId)
            .GreaterThan(0).WithMessage("EventId must be greater than 0.");
    }

}

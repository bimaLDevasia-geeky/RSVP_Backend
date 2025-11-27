using System;
using FluentValidation;

namespace RSVP.Application.Features.Images.Commands.DeleteImagesInGroup;

public class DeleteImagesInGroupValidator:AbstractValidator<DeleteImagessInGroupCommand>
{
    public DeleteImagesInGroupValidator()
    {
        RuleFor(x => x.ImageIds)
            .NotEmpty().WithMessage("ImageIds list cannot be empty.")
            .Must(ids => ids.All(id => id > 0)).WithMessage("All ImageIds must be positive integers.");
            
    }
}

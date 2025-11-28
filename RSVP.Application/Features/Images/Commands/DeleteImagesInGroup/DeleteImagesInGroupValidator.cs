using System;
using System.Data;
using FluentValidation;

namespace RSVP.Application.Features.Images.Commands.DeleteImagesInGroup;

public class DeleteImagesInGroupValidator:AbstractValidator<DeleteImagesInGroupCommand>
{
    public DeleteImagesInGroupValidator()
    {
        RuleFor(x => x.ImageIds)
            .NotEmpty().WithMessage("ImageIds list cannot be empty.")
            .Must(ids => ids.Select(i => i.ImageId).All(id => id > 0)).WithMessage("All ImageIds must be positive integers.");
        RuleForEach(x => x.ImageIds).ChildRules(image =>
        {
            image.RuleFor(i => i.PublicId)
                .NotEmpty().WithMessage("PublicId cannot be empty.");
        });
            
    }
}

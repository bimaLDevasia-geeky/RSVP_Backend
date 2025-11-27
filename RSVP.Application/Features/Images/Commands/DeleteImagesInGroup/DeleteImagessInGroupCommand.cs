using System;
using MediatR;

namespace RSVP.Application.Features.Images.Commands.DeleteImagesInGroup;

public class DeleteImagessInGroupCommand:IRequest<bool>
{
    public List<int> ImageIds { get; set; } = new List<int>();
}

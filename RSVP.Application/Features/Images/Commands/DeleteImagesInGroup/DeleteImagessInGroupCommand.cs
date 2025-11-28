using System;
using MediatR;
using RSVP.Application.Dtos;

namespace RSVP.Application.Features.Images.Commands.DeleteImagesInGroup;

public class DeleteImagesInGroupCommand:IRequest<bool>
{
    public List<ImageDeleteDto> ImageIds { get; set; } = new List<ImageDeleteDto>();
}

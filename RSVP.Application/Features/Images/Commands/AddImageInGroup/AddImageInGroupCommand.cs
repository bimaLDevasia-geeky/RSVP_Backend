using System;
using MediatR;

namespace RSVP.Application.Features.Images.Commands.AddImageInGroup;

public class AddImageInGroupCommand:IRequest<bool>
{
    public int EventId { get; set; }
    public List<string> ImageUrls { get; set; } = new List<string>();
}

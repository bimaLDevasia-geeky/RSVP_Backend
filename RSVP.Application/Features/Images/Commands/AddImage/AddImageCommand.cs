using System;
using MediatR;

namespace RSVP.Application.Features.Images.Commands.AddImage;

public class AddImageCommand:IRequest<bool>
{
    public int EventId { get; set; }
    public string ImageUrl { get; set; } = null!;
}

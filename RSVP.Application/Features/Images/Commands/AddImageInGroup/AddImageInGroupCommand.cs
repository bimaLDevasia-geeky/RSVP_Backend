using System;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace RSVP.Application.Features.Images.Commands.AddImageInGroup;

public class AddImageInGroupCommand:IRequest<bool>
{
    public int EventId { get; set; }
    public List<IFormFile> Files { get; set; } = new List<IFormFile>();
}

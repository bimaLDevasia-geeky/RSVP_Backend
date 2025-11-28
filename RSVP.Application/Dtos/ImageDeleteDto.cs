using System;

namespace RSVP.Application.Dtos;

public class ImageDeleteDto
{
    public int ImageId { get; set; }
    public string PublicId { get; set; } = null!;
}

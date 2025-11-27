using System;
using System.Text.Json.Serialization;
using MediatR;

namespace RSVP.Application.Features.Images.Commands.DeleteImage;

public class DeleteImageCommand:IRequest<bool>
{
    [JsonIgnore]
    public int ImageId { get; set; }
 

}

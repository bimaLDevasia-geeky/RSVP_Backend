using System.Text.Json.Serialization;
using MediatR;

namespace RSVP.Application.Features.Event.Commands.DeleteEvent;

public record class DeleteEventCommand:IRequest<bool>
{  
     [JsonIgnore]
    public int EventId { get; set; }
}

using System.Text.Json.Serialization;
using MediatR;
using RSVP.Domain.Enums;

namespace RSVP.Application.Features.Event.Commands.UpdateEvent;

public record class UpdateEventCommand:IRequest<bool>
{  
    [JsonIgnore] 
    public int EventId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime? Date { get; set; }
    public string? Venue { get; set; } 
    public TimeOnly? Time { get; set; }
    public bool? IsPublic { get; set; }

    public EventStatus? Status { get; set; }
}

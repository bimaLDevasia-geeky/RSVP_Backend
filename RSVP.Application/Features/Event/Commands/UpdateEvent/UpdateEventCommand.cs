using System.Text.Json.Serialization;
using MediatR;
using Microsoft.AspNetCore.Http;
using RSVP.Application.Dtos;
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
    
    public List<ImageDeleteDto>? ImagesToDelete { get; set; }
    public List<IFormFile>? NewImages { get; set; }
}

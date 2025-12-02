
using System.Text.Json.Serialization;
using MediatR;
using RSVP.Domain.Enums;

namespace RSVP.Application.Features.Attendie.Command.UpdateAttendie;

public class UpdateAttendieCommand:IRequest<bool>
{
    [JsonIgnore]
    public int AttendieId { get; set; }
    public AttendiesRole? Role { get; set; }
    public AttendiesStatus? Status { get; set; }
    
}

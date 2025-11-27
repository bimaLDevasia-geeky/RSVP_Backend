using System;
using System.Text.Json.Serialization;
using MediatR;

namespace RSVP.Application.Features.Attendie.Command.DeleteAttendie;

public class DeleteAttendieCommand:IRequest<bool>
{  
    [JsonIgnore]
    public int AttendieId { get; set; }
}

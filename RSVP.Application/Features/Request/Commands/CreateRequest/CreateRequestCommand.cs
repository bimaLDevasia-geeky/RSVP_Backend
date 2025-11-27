using System;
using MediatR;

namespace RSVP.Application.Features.Request.Commands.CreateRequest;

public class CreateRequestCommand:IRequest<int>
{
    public int EventId { get; set; }

}

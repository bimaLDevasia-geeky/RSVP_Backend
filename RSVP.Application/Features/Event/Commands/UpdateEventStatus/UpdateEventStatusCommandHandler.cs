using System;
using MediatR;
using RSVP.Application.Interfaces;

namespace RSVP.Application.Features.Event.Commands.UpdateEventStatus;

public class UpdateEventStatusCommandHandler:IRequestHandler<UpdateEventStatusCommand,bool>
{

    private readonly IRsvpDbContext _context;
    private readonly IRepository<Domain.Entities.Event> _eventRepository;

    public UpdateEventStatusCommandHandler(IRsvpDbContext context, IRepository<Domain.Entities.Event> eventRepository)
    {
        _context = context;
        _eventRepository = eventRepository;
    }

    public async Task<bool> Handle(UpdateEventStatusCommand request, CancellationToken cancellationToken)
    {
        var eventEntity = await _eventRepository.GetByIdAsync(request.EventId, cancellationToken);
        if (eventEntity == null)
        {
            return false;
            throw new KeyNotFoundException($"Event with ID {request.EventId} not found.");
        }

        eventEntity.UpdateEvent(null,null,null,null,null,null,status: request.Status);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}

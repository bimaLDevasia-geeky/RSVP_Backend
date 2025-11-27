using System;
using MediatR;
using RSVP.Application.Interfaces;
using appDomain=RSVP.Domain.Entities;

namespace RSVP.Application.Features.Event.Commands.DeleteEvent;

public class DeleteEventCommandHandler:IRequestHandler<DeleteEventCommand,bool>
{
    private readonly IEventAccessService _eventAccessService;
    private readonly IRepository<appDomain.Event> _eventRepository;

    public DeleteEventCommandHandler(IEventAccessService eventAccessService, IRepository<appDomain.Event> eventRepository)
    {
        _eventAccessService = eventAccessService;
        _eventRepository = eventRepository;
    }

    public async Task<bool> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
    {
        bool hasAccess = await _eventAccessService.IsOrganizerOrOwnerAsync(request.EventId, cancellationToken);
        if (!hasAccess)
        {
            throw new UnauthorizedAccessException("User is not authorized to delete this event.");
        }
            appDomain.Event? eventToDelete = await _eventRepository.GetByIdAsync(request.EventId, cancellationToken);
        if (eventToDelete is null)
        {
            throw new KeyNotFoundException("Event not found.");
        }
         _eventRepository.Delete(eventToDelete);

        await _eventRepository.SaveChangesAsync(cancellationToken);

        return true; // Return true if deletion was successful.
    }
}

using System;
using MediatR;
using RSVP.Application.Interfaces;
using appDomain=RSVP.Domain.Entities;

namespace RSVP.Application.Features.Event.Commands.UpdateEvent;

public class UpdateEventCommandHandler:IRequestHandler<UpdateEventCommand,bool>
{
    private readonly IEventAccessService _eventAccessService;
    private readonly IRepository<appDomain.Event> _eventRepository;
    public UpdateEventCommandHandler(IEventAccessService eventAccessService, IRepository<appDomain.Event> eventRepository)
    {
        _eventAccessService = eventAccessService;
        _eventRepository = eventRepository;
    }
    public async Task<bool> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
    {
        Boolean isOrganizerOrOwner = await _eventAccessService.IsOrganizerOrOwnerAsync(request.EventId, cancellationToken);
        if (!isOrganizerOrOwner)
        {
            throw new UnauthorizedAccessException("User is not authorized to update this event.");
        }

        appDomain.Event? eventToUpdate = await _eventRepository.GetByIdAsync(request.EventId, cancellationToken);

        if (eventToUpdate is null)
        {
            throw new KeyNotFoundException("Event not found.");
        }

        eventToUpdate.UpdateEvent(request.Name, request.Description, request.Date, request.Venue, request.Time, request.IsPublic,request.Status);
        await _eventRepository.SaveChangesAsync(cancellationToken);
        return true;
    }
}

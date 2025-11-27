using System;
using MediatR;
using RSVP.Application.Interfaces;
using appDomain = RSVP.Domain.Entities;

namespace RSVP.Application.Features.Event.Commands.CreateEvent;

public class CreateEventCommandHandler:IRequestHandler<CreateEventCommand,int>
{       
    private readonly ICurrentUser _currentUser;
    private readonly IRepository<appDomain.Event> _eventRepository;
    public CreateEventCommandHandler(ICurrentUser currentUser, IRepository<appDomain.Event> eventRepository)
    {
        _currentUser = currentUser;
        _eventRepository = eventRepository;
    }   
    public async Task<int> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {

        int createdBy  = _currentUser.UserId;
        appDomain.Event newEvent = new appDomain.Event(
            request.Name,
            request.Description,
            createdBy,
            request.Date,
            request.Venue,
            request.Time,
            request.IsPublic
            );


        await _eventRepository.AddAsync(newEvent,cancellationToken);  
        await _eventRepository.SaveChangesAsync(cancellationToken);
        return newEvent.Id;

    }
}

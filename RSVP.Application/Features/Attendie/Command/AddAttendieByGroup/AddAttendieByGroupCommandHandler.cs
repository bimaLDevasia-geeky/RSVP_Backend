using System;
using MediatR;
using RSVP.Application.Interfaces;

namespace RSVP.Application.Features.Attendie.Command.AddAttendieByGroup;

public class AddAttendieByGroupCommandHandler:IRequestHandler<AddAttendieByGroupCommand,bool>
{   
    private readonly IRepository<Domain.Entities.Attendie> _attendieRepository;
    private readonly IRepository<Domain.Entities.Event> _eventRepository;
    private readonly IEventAccessService _eventAccessService;   
    public AddAttendieByGroupCommandHandler(IRepository<Domain.Entities.Attendie> attendieRepository, IEventAccessService eventAccessService, IRepository<Domain.Entities.Event> eventRepository)
    {
        _attendieRepository = attendieRepository;
        _eventRepository = eventRepository;
        _eventAccessService = eventAccessService;
    }
    public async Task<bool> Handle(AddAttendieByGroupCommand request, CancellationToken cancellationToken)
    {

        var eventExists = await _eventRepository.GetByIdAsync(request.EventId, cancellationToken);
        if (eventExists == null)
        {
            throw new KeyNotFoundException($"Event with ID {request.EventId} not found.");
        }
        Boolean isOrganizerOrOwner = await _eventAccessService.IsOrganizerOrOwnerAsync(request.EventId, cancellationToken);
        if (!isOrganizerOrOwner)
        {
            throw new UnauthorizedAccessException("User is not authorized to add attendies to this event.");
        }

        HashSet<string> incomingEmails = request.AttendieEmails.ToHashSet();

        var existingEmails =  _attendieRepository.QuerableAsync(cancellationToken)
        .Where(a => a.EventId == request.EventId && incomingEmails.Contains(a.Email))
        .Select(a => a.Email.ToLower());

        List<string> newAttendies = incomingEmails.Except(existingEmails).ToList();
            
        List<Domain.Entities.Attendie> attendiesToAdd = newAttendies
        .Select(email => new Domain.Entities.Attendie
        (
            eventId : request.EventId,
            email : email,
            userId : null
        ))
        .ToList();
        
        if (attendiesToAdd.Any())
        {
            await _attendieRepository.AddRangeAsync(attendiesToAdd, cancellationToken);
            await _attendieRepository.SaveChangesAsync(cancellationToken);
        }
        

        throw new NotImplementedException();
    }
}

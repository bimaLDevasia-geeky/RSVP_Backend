using System;
using MediatR;
using RSVP.Application.Interfaces;
using appDomain = RSVP.Domain.Entities;

namespace RSVP.Application.Features.Attendie.Command.AddAttendie;

public class AddAttendieCommandHandler:IRequestHandler<AddAttendieCommand,int>
{   
    private readonly IEventAccessService _eventAccessService;
    private readonly IRepository<RSVP.Domain.Entities.Attendie> _attendieRepository;
    private readonly IUserReposistory _userRepository;
    public AddAttendieCommandHandler(IEventAccessService eventAccessService, IUserReposistory userRepository, IRepository<RSVP.Domain.Entities.Attendie> attendieRepository)
    {
        _eventAccessService = eventAccessService;
        _attendieRepository = attendieRepository;
        _userRepository = userRepository;
    }
    
    public async Task<int> Handle(AddAttendieCommand request, CancellationToken cancellationToken)
    {

        Boolean isOrganizerOrOwner =  await _eventAccessService.IsOrganizerOrOwnerAsync( request.EventId, cancellationToken);
        if (!isOrganizerOrOwner)
        {
            throw new UnauthorizedAccessException("User is not authorized to add attendie to this event.");
        }
        appDomain.User? user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {request.UserId} not found.");
        }
        appDomain.Attendie newAttendie = new (
            request.EventId,
            request.UserId,
            user.Email
            );
        await _attendieRepository.AddAsync(newAttendie,cancellationToken);
        await  _attendieRepository.SaveChangesAsync(cancellationToken);
        return newAttendie.Id;
    }
}

using System;
using MediatR;
using RSVP.Application.Interfaces;
using RSVP.Domain.Enums;
using appDomain = RSVP.Domain.Entities;
namespace RSVP.Application.Features.Invite.Commands;

public class AcceptInviteCommandHandler:IRequestHandler<AcceptInviteCommand,bool>
{
    private readonly IAttendieRepository _attendieRepository;
    private readonly IRepository<appDomain.Request> _requestRepository;
    private readonly IRepository<appDomain.User> _userRepository;
    private readonly IEventRepository _eventRepository;
    private readonly ICurrentUser _currentUser;
    public AcceptInviteCommandHandler(IAttendieRepository attendieRepository, IRepository<appDomain.Request> requestRepository, IRepository<appDomain.User> userRepository, IEventRepository eventRepository, ICurrentUser currentUser)
    {
        _attendieRepository = attendieRepository;
        _requestRepository = requestRepository;
        _userRepository = userRepository;
        _eventRepository = eventRepository;
        _currentUser = currentUser;
    }

    public async Task<bool> Handle(AcceptInviteCommand request, CancellationToken cancellationToken)
    {
        appDomain.Event? evnt = await _eventRepository.GetEventByInviteCodeAsync(request.InviteCode, cancellationToken);

        if (evnt is null)
        {
            throw new Exception("Invalid Invite Code");
        }
        appDomain.User? user = await _userRepository.GetByIdAsync(_currentUser.UserId, cancellationToken);
        
        if(user is null)
        {
            throw new UnauthorizedAccessException("User not found");
        }
        appDomain.Attendie? existingAttendie = await _attendieRepository.GetAttendieByEmailAndEventIdAsync(user.Email, evnt.Id, cancellationToken);  

        if(evnt.IsPublic == false)
        {
            if(existingAttendie is null)
            {
                appDomain.Request req =new(
                    evnt.Id,
                    user.Id
                );
            }
            else
            {
                existingAttendie.UpdateUserId(user.Id);
                existingAttendie.UpdateStatus(request.Status);
            }
            
        }
        else
        {
            if(existingAttendie is null)
            {
                appDomain.Attendie newAttendie = new appDomain.Attendie(
                    evnt.Id,
                    user.Id,
                    user.Email
                );
                newAttendie.UpdateStatus(request.Status);
                await _attendieRepository.AddAsync(newAttendie, cancellationToken);
               
            }
            else{
                existingAttendie.UpdateUserId(user.Id);
                existingAttendie.UpdateStatus(request.Status);
            }
            
        }
        await _attendieRepository.SaveChangesAsync(cancellationToken);
        return true;
        
    }
}

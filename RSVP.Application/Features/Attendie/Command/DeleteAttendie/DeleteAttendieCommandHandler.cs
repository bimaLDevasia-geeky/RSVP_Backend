using System;
using MediatR;
using RSVP.Application.Interfaces;
using appDomain=RSVP.Domain.Entities;
namespace RSVP.Application.Features.Attendie.Command.DeleteAttendie;

public class DeleteAttendieCommandHandler:IRequestHandler<DeleteAttendieCommand,bool>
{   
    private readonly IRepository<appDomain.Attendie> _attendieRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IEventAccessService _eventAccessService;
    public DeleteAttendieCommandHandler(IRepository<appDomain.Attendie> attendieRepository, ICurrentUser currentUser, IEventAccessService eventAccessService)
    {
        _attendieRepository = attendieRepository;
        _currentUser = currentUser;
        _eventAccessService = eventAccessService;
    }
    public async Task<bool> Handle(DeleteAttendieCommand request, CancellationToken cancellationToken)
    {
        appDomain.Attendie? attendie =  await _attendieRepository.GetByIdAsync(request.AttendieId, cancellationToken);
        if(attendie == null)
        {
            throw new Exception("Attendie not found");
        }
        bool hasAccess =  await _eventAccessService.IsOrganizerOrOwnerAsync(attendie.EventId, cancellationToken);
        if(attendie.UserId != _currentUser.UserId && !hasAccess)
        {
            throw new UnauthorizedAccessException("You do not have permission to delete this attendie");
        }
         _attendieRepository.Delete(attendie);
        await _attendieRepository.SaveChangesAsync(cancellationToken);
        return true;
    }
}

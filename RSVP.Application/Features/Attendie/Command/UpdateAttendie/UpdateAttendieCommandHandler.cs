using System;
using MediatR;
using RSVP.Application.Interfaces;
using appDomain=RSVP.Domain.Entities;
namespace RSVP.Application.Features.Attendie.Command.UpdateAttendie;

public class UpdateAttendieCommandHandler:IRequestHandler<UpdateAttendieCommand,bool>
{
    private readonly IRepository<appDomain.Attendie> _attendieRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IEventAccessService _eventAccessService;
    public UpdateAttendieCommandHandler(IRepository<appDomain.Attendie> attendieRepository, ICurrentUser currentUser, IEventAccessService eventAccessService)
    {
        _attendieRepository = attendieRepository;
        _currentUser = currentUser;
        _eventAccessService = eventAccessService;
    }

    public async Task<bool> Handle(UpdateAttendieCommand request, CancellationToken cancellationToken)
    {
        var attendie = await _attendieRepository.GetByIdAsync(request.AttendieId, cancellationToken);
        if (attendie == null)
        {
            throw new KeyNotFoundException("Attendie not found");
        }

        var hasAccess = await _eventAccessService.IsOrganizerOrOwnerAsync(attendie.EventId, cancellationToken);
        if ((attendie.UserId == _currentUser.UserId || hasAccess) && request.Status.HasValue)
        {
            attendie.UpdateStatus(request.Status.Value);
        }
         else
        {
            throw new UnauthorizedAccessException("You do not have permission to update this attendie");
           
        }
        if(hasAccess && request.Role.HasValue)
        {
            attendie.UpdateRole(request.Role.Value);
        }
        else
        {
            throw new UnauthorizedAccessException("You do not have permission to update this attendie");
           
        }


        await _attendieRepository.SaveChangesAsync( cancellationToken);
        return true;
        
    }
}

using System;
using MediatR;
using RSVP.Application.Interfaces;
using RSVP.Domain.Enums;
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
        var isOwnAttendee = attendie.UserId == _currentUser.UserId;
        var isOwnAttendeeEmail = attendie.Email == _currentUser.Email;


        // Update status - allowed if user is the attendee or has organizer/owner access
        if (request.Status.HasValue)
        {
            if (!isOwnAttendee && !isOwnAttendeeEmail && !hasAccess)
            {
                throw new UnauthorizedAccessException("You do not have permission to update this attendee's status"+isOwnAttendeeEmail);
            }
            attendie.UpdateStatus(request.Status.Value);
        }
            if (isOwnAttendee && request.Status !=AttendiesStatus.NoResponse)
            attendie.UpdateUserId(_currentUser.UserId);
        // Update role - only allowed for organizers/owners
        if (request.Role.HasValue)
        {
            if (!hasAccess)
            {
                throw new UnauthorizedAccessException("You do not have permission to update this attendee's role");
            }
            attendie.UpdateRole(request.Role.Value);
        }

        await _attendieRepository.SaveChangesAsync(cancellationToken);
        return true;
    }
}

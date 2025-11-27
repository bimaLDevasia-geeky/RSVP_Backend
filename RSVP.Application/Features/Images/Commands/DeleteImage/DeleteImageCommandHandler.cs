using System;
using MediatR;
using RSVP.Application.Interfaces;
using appDomain = RSVP.Domain.Entities;
namespace RSVP.Application.Features.Images.Commands.DeleteImage;

public class DeleteImageCommandHandler:IRequestHandler<DeleteImageCommand,bool>
{
    private readonly IRepository<appDomain.Media> _mediarepository;
    private readonly IEventAccessService _eventAccessService;
    public DeleteImageCommandHandler(IRepository<appDomain.Media> mediarepository, IEventAccessService eventAccessService)
    {
        _mediarepository = mediarepository;
        _eventAccessService = eventAccessService;
    }
    public async Task<bool> Handle(DeleteImageCommand request, CancellationToken cancellationToken)
    {
        appDomain.Media? imageToDelete = await _mediarepository.GetByIdAsync(request.ImageId, cancellationToken);
        if (imageToDelete == null)
        {
            throw new KeyNotFoundException($"Image with ID {request.ImageId} not found.");
        }


        Boolean isOrganizerOrOwner = await _eventAccessService.IsOrganizerOrOwnerAsync(imageToDelete.EventId, cancellationToken);
        if (!isOrganizerOrOwner)
        {
            throw new UnauthorizedAccessException("User is not authorized to delete image from this event.");
        }
        _mediarepository.Delete(imageToDelete);
        await  _mediarepository.SaveChangesAsync(cancellationToken);
        return true;
    }
}


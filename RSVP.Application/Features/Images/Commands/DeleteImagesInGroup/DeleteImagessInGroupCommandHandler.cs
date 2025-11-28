using System;
using MediatR;
using RSVP.Application.Interfaces;
using appDomain = RSVP.Domain.Entities;

namespace RSVP.Application.Features.Images.Commands.DeleteImagesInGroup;

public class DeleteImagessInGroupCommandHandler:IRequestHandler<DeleteImagesInGroupCommand,bool>
{
    private readonly IRepository<appDomain.Media> _mediarepository;
    private readonly IEventAccessService _eventAccessService;
    private readonly IRsvpDbContext _dbContext;
    private readonly IPhotoService _photoService;
    public DeleteImagessInGroupCommandHandler(IRepository<appDomain.Media> mediarepository, IEventAccessService eventAccessService, IRsvpDbContext dbContext, IPhotoService photoService)
    {
        _mediarepository = mediarepository;
        _eventAccessService = eventAccessService;
        _dbContext = dbContext;
        _photoService = photoService;
    }
    public async Task<bool> Handle(DeleteImagesInGroupCommand request, CancellationToken cancellationToken)
    {
       List<appDomain.Media> imagesToDelete = _dbContext.Media.Where(m => request.ImageIds.Select(i => i.ImageId).Contains(m.Id)).ToList();

        if (imagesToDelete.Count == 0)
        {
            throw new KeyNotFoundException("No images found for the provided IDs.");
        }

        
        List<int> distinctEventIds = imagesToDelete.Select(img => img.EventId).Distinct().ToList();

         if (distinctEventIds.Count > 1)
         {
            throw new ArgumentException("Batch deletion is only allowed for a single event at a time.");
        }

        int eventId = distinctEventIds.First();

        Boolean isOrganizerOrOwner = await _eventAccessService.IsOrganizerOrOwnerAsync(eventId, cancellationToken);
        if (!isOrganizerOrOwner)
        {
            throw new UnauthorizedAccessException("User is not authorized to delete images from this event.");
        }
        long count = await _photoService.DeleteMultiplePhotosAsync(request.ImageIds.Select(i => i.PublicId).ToList());
        if(count == 0)
        {
            throw new Exception("Failed to delete images from the photo service.");
        }

        _mediarepository.DeleteRange(imagesToDelete);
        await  _mediarepository.SaveChangesAsync(cancellationToken);
        return true;
    }
}

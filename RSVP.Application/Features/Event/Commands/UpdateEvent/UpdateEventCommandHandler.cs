using System;
using MediatR;
using RSVP.Application.Interfaces;
using appDomain=RSVP.Domain.Entities;

namespace RSVP.Application.Features.Event.Commands.UpdateEvent;

public class UpdateEventCommandHandler:IRequestHandler<UpdateEventCommand,bool>
{
    private readonly IEventAccessService _eventAccessService;
    private readonly IRepository<appDomain.Event> _eventRepository;
    private readonly IRepository<appDomain.Media> _mediaRepository;
    private readonly IPhotoService _photoService;
    private readonly IRsvpDbContext _dbContext;
    
    public UpdateEventCommandHandler(
        IEventAccessService eventAccessService, 
        IRepository<appDomain.Event> eventRepository,
        IRepository<appDomain.Media> mediaRepository,
        IPhotoService photoService,
        IRsvpDbContext dbContext)
    {
        _eventAccessService = eventAccessService;
        _eventRepository = eventRepository;
        _mediaRepository = mediaRepository;
        _photoService = photoService;
        _dbContext = dbContext;
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

        eventToUpdate.UpdateEvent(request.Name, request.Description, request.Date, request.Venue, request.Time, request.IsPublic, request.Status);
        await _eventRepository.SaveChangesAsync(cancellationToken);
        
        // Delete images if provided
        if (request.ImagesToDelete != null && request.ImagesToDelete.Any())
        {
            List<appDomain.Media> imagesToDelete = _dbContext.Media
                .Where(m => request.ImagesToDelete.Select(i => i.ImageId).Contains(m.Id))
                .ToList();

            if (imagesToDelete.Any())
            {
                // Verify all images belong to this event
                if (imagesToDelete.Any(img => img.EventId != request.EventId))
                {
                    throw new UnauthorizedAccessException("Cannot delete images from other events.");
                }

                // Delete from cloud storage
                long deleteCount = await _photoService.DeleteMultiplePhotosAsync(
                    request.ImagesToDelete.Select(i => i.PublicId).ToList()
                );
                
                if (deleteCount > 0)
                {
                    _mediaRepository.DeleteRange(imagesToDelete);
                    
                }
            }
        }
        if(request.NewImages != null && request.NewImages.Any())
        {
            IEnumerable<CloudinaryDotNet.Actions.ImageUploadResult> uploadResults = await _photoService.AddPhotosAsync(request.NewImages);
            if (uploadResults == null || !uploadResults.Any())
            {
                throw new Exception("Failed to upload new images.");
            }

            List<appDomain.Media> newImages = uploadResults.Select(result => new appDomain.Media
            (
                eventId: request.EventId,
                url: result.SecureUrl.ToString(),
                publicId: result.PublicId
            )).ToList();

            await _mediaRepository.AddRangeAsync(newImages, cancellationToken);
           
        }
         await _mediaRepository.SaveChangesAsync(cancellationToken);
        return true;
    }
}

using System;
using CloudinaryDotNet.Actions;
using MediatR;
using RSVP.Application.Interfaces;
using RSVP.Domain.Entities;

namespace RSVP.Application.Features.Images.Commands.AddImageInGroup;

public class AddImageInGroupCommandHandler:IRequestHandler<AddImageInGroupCommand,bool>
{
    private readonly IRepository<Media> _imageRepository;
    private readonly IEventAccessService _eventAccessService;
    private readonly IPhotoService _photoService;
    public AddImageInGroupCommandHandler(IRepository<Media> imageRepository, IEventAccessService eventAccessService, IPhotoService photoService)
    {
        _imageRepository = imageRepository;
        _eventAccessService = eventAccessService;
        _photoService = photoService;
    }

    public async Task<bool> Handle(AddImageInGroupCommand request, CancellationToken cancellationToken)
    {
        Boolean isOrganizerOrOwner = await _eventAccessService.IsOrganizerOrOwnerAsync(request.EventId, cancellationToken);
        if (!isOrganizerOrOwner)
        {
            throw new UnauthorizedAccessException("User is not authorized to add image to this event.");
        }

        IEnumerable<ImageUploadResult> uploadResults = await _photoService.AddPhotosAsync(request.Files);
        if (uploadResults == null || !uploadResults.Any())
        {
            throw new Exception("Failed to upload images.");
        }

        List<Media> images = uploadResults.Select(result => new Media
        (
            eventId:request.EventId,
            url: result.SecureUrl.ToString(),
            publicId: result.PublicId
        )).ToList();

        await  _imageRepository.AddRangeAsync(images, cancellationToken);
        await _imageRepository.SaveChangesAsync(cancellationToken);
        return true;
    }
}

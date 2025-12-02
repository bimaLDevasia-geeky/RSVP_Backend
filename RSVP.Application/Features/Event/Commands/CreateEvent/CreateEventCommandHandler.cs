using System;
using CloudinaryDotNet.Actions;
using MediatR;
using RSVP.Application.Interfaces;
using appDomain = RSVP.Domain.Entities;

namespace RSVP.Application.Features.Event.Commands.CreateEvent;

public class CreateEventCommandHandler:IRequestHandler<CreateEventCommand,int>
{       
    private readonly ICurrentUser _currentUser;
    private readonly IRepository<appDomain.Event> _eventRepository;
    private readonly IRepository<appDomain.Media> _mediaRepository;
    private readonly IPhotoService _photoService;
    
    public CreateEventCommandHandler(
        ICurrentUser currentUser, 
        IRepository<appDomain.Event> eventRepository,
        IRepository<appDomain.Media> mediaRepository,
        IPhotoService photoService)
    {
        _currentUser = currentUser;
        _eventRepository = eventRepository;
        _mediaRepository = mediaRepository;
        _photoService = photoService;
    }   
    
    public async Task<int> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        int createdBy  = _currentUser.UserId;
        appDomain.Event newEvent = new appDomain.Event(
            request.Name,
            request.Description,
            createdBy,
            request.Date,
            request.Venue,
            request.Time,
            request.IsPublic
            );

        await _eventRepository.AddAsync(newEvent,cancellationToken);  
        await _eventRepository.SaveChangesAsync(cancellationToken);
        
        // Add images if provided
        if (request.Images != null && request.Images.Any())
        {
            IEnumerable<ImageUploadResult> uploadResults = await _photoService.AddPhotosAsync(request.Images);
            if (uploadResults != null && uploadResults.Any())
            {
                List<appDomain.Media> images = uploadResults.Select(result => new appDomain.Media(
                    eventId: newEvent.Id,
                    url: result.SecureUrl.ToString(),
                    publicId: result.PublicId
                )).ToList();

                await _mediaRepository.AddRangeAsync(images, cancellationToken);
                await _mediaRepository.SaveChangesAsync(cancellationToken);
            }
        }
        
        return newEvent.Id;
    }
}

using System;
using MediatR;
using RSVP.Application.Interfaces;
using appDomain = RSVP.Domain.Entities;
namespace RSVP.Application.Features.Images.Commands.AddImage;

public class AddImageCommandHandler:IRequestHandler<AddImageCommand,bool>
{
    private readonly IRepository<appDomain.Media> _mediarepository;
    private readonly IEventAccessService _eventAccessService;
    public AddImageCommandHandler(IRepository<appDomain.Media> mediarepository, IEventAccessService eventAccessService)
    {
        _mediarepository = mediarepository;
        _eventAccessService = eventAccessService;
    }
    public async Task<bool> Handle(AddImageCommand request, CancellationToken cancellationToken)
    {
        Boolean isOrganizerOrOwner = await _eventAccessService.IsOrganizerOrOwnerAsync(request.EventId, cancellationToken);
        if (!isOrganizerOrOwner)
        {
            throw new UnauthorizedAccessException("User is not authorized to add image to this event.");
        }
        
        appDomain.Media newImage = new (
            request.EventId,
            request.ImageUrl,
            publicId:"dms"
            );
        await _mediarepository.AddAsync(newImage,cancellationToken);
        
        await  _mediarepository.SaveChangesAsync(cancellationToken);
        return true;
    }
}

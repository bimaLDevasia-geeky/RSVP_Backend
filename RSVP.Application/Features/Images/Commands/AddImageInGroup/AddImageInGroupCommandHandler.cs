using System;
using MediatR;
using RSVP.Application.Interfaces;
using RSVP.Domain.Entities;

namespace RSVP.Application.Features.Images.Commands.AddImageInGroup;

public class AddImageInGroupCommandHandler:IRequestHandler<AddImageInGroupCommand,bool>
{
    private readonly IRepository<Media> _imageRepository;
    public AddImageInGroupCommandHandler(IRepository<Media> imageRepository)
    {
        _imageRepository = imageRepository;
    }

    public async Task<bool> Handle(AddImageInGroupCommand request, CancellationToken cancellationToken)
    {
        List<Media> images = request.ImageUrls.Select(url => new Media
        (
            eventId:request.EventId,
            url: url
        )).ToList();

        await  _imageRepository.AddRangeAsync(images, cancellationToken);
        await _imageRepository.SaveChangesAsync(cancellationToken);
        return true;
    }
}

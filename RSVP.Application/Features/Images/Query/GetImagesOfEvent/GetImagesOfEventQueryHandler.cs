using System;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RSVP.Application.Interfaces;
using RSVP.Domain.Entities;

namespace RSVP.Application.Features.Images.Query.GetImagesOfEvent;

public class GetImagesOfEventQueryHandler:IRequestHandler<GetImagesOfEventQuery, List<Media>>
{
    private readonly IRsvpDbContext _context;

    public GetImagesOfEventQueryHandler(IRsvpDbContext context)
    {
        _context = context;
    }

    public async Task<List<Media>> Handle(GetImagesOfEventQuery request, CancellationToken cancellationToken)
    {
       
        return await _context.Media
            .Where(media => media.EventId == request.EventId)
            .ToListAsync(cancellationToken);
        
    }
}

using System;
using MediatR;
using RSVP.Domain.Entities;

namespace RSVP.Application.Features.Images.Query.GetImagesOfEvent;

public class GetImagesOfEventQuery:IRequest<List<Media>>
{
    public int EventId { get; set; }
}

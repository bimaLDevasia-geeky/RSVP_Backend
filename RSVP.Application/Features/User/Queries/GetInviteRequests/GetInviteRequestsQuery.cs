using System;
using MediatR;
using RSVP.Application.Dtos;

namespace RSVP.Application.Features.User.Queries.GetInviteRequests;

public class GetInviteRequestsQuery:IRequest<List<InviteRequestDto>>
{
    public string term { get; set; }=null!;
}

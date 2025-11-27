using System;
using MediatR;
using RSVP.Application.Dtos;

namespace RSVP.Application.Features.Auth.Commands.RefreshToken;

public class RefreshAccessTokenCommand:IRequest<RefreshDTO>
{
    
}

using System;
using System.Net;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RSVP.Application.Dtos;
using RSVP.Application.Interfaces;
using RSVP.Domain.Enums;
using appDomain= RSVP.Domain.Entities;

namespace RSVP.Application.Features.Auth.Commands.RefreshToken;

public class RefreshAccessTokenCommandHandler:IRequestHandler<RefreshAccessTokenCommand,RefreshDTO>
{
    private readonly IRefreshReposistary _refreshTokenRepository;
    private readonly ICurrentUser   _currentUser;
    private readonly IRepository<appDomain.User> _userRepository;   
    private readonly ITokenService _tokenService;
    public RefreshAccessTokenCommandHandler(IRefreshReposistary refreshTokenRepository, ICurrentUser currentUser, ITokenService tokenService, IRepository<appDomain.User> userRepository)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _currentUser = currentUser;
        _tokenService = tokenService;
        _userRepository = userRepository;
    }
    public async Task<RefreshDTO> Handle(RefreshAccessTokenCommand request, CancellationToken cancellationToken)
    {
            string refreshToken = _currentUser.RefreshToken;
            appDomain.RefreshToken? dbtoken = await _refreshTokenRepository.GetByToken(refreshToken);
            if (dbtoken is null || dbtoken.ExpiresAt < DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException("Invalid or expired refresh token.");
            }
            appDomain.User? user = await _userRepository.GetByIdAsync(dbtoken.UserId,cancellationToken);

            if (user is null)
            {
                throw new UnauthorizedAccessException("User not found.");
            }

            dbtoken.Revoke();
            string role;
        if (user.Role == UserRole.Admin)
        {
            role ="Admin";
        }
        else
        {
            role ="User";
        }
    
            string newAccessToken = _tokenService.GenerateAccessToken(dbtoken.UserId, user.Email, role);

            appDomain.RefreshToken newRefreshToken = _tokenService.GenerateRefreshToken(dbtoken.UserId);
            _tokenService.SetRefreshTokenInCookies(newRefreshToken);
            await _refreshTokenRepository.AddAsync(newRefreshToken,cancellationToken);
            await _refreshTokenRepository.SaveChangesAsync(cancellationToken);


            RefreshDTO result = new(newAccessToken);
            return result;
    }
}

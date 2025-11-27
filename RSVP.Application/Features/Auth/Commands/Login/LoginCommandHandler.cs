using System;
using System.Data.Common;
using System.Security.Principal;
using MediatR;
using Microsoft.Extensions.Options;
using RSVP.Application.Dtos;
using RSVP.Application.Interfaces;
using RSVP.Domain.Entities;
using appDomain= RSVP.Domain.Entities;

namespace RSVP.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler:IRequestHandler<LoginCommand,LoginResultDto>
{
    private readonly ITokenService tokenService;
    private readonly IUserReposistory _userRepository;
    private readonly IRepository<appDomain.RefreshToken> _refreshTokenRepository;
    
    public LoginCommandHandler(ITokenService tokenService, IUserReposistory userRepository, IRepository<appDomain.RefreshToken> refreshTokenRepository)
    {
        this.tokenService = tokenService;
        this._userRepository = userRepository;
        this._refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<LoginResultDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        
        appDomain.User? user = await _userRepository.GetUserByEmailAsync(request.Email, cancellationToken);
        if (user == null || BCrypt.Net.BCrypt.Verify(request.Password, user.HashedPassword) == false)
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }
        string accessToken = tokenService.GenerateAccessToken(user.Id, request.Email, "User");

       appDomain.RefreshToken refreshToken = tokenService.GenerateRefreshToken(user.Id);

        tokenService.SetRefreshTokenInCookies(refreshToken);

        appDomain.RefreshToken refreshTokenEntity = new(user.Id, refreshToken.Token, refreshToken.ExpiresAt);
        
        await _refreshTokenRepository.AddAsync(refreshTokenEntity, cancellationToken);
        LoginResultDto result = new (id: user.Id, email: user.Email, token: accessToken, role: user.Role);
        
        
        await _refreshTokenRepository.SaveChangesAsync(cancellationToken);
        return result;
    }
}

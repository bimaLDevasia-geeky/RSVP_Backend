using MediatR;
using RSVP.Application.Interfaces;

namespace RSVP.Application.Features.Auth.Commands.Logout;

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, bool>
{
    private readonly ITokenService _tokenService;
    private readonly ICurrentUser _currentUser;
    private readonly IRefreshReposistary _refreshTokenRepository;

    public LogoutCommandHandler(ITokenService tokenService, ICurrentUser currentUser, IRefreshReposistary refreshTokenRepository)
    {
        _tokenService = tokenService;
        _currentUser = currentUser;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<bool> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        // Get refresh token from cookies
        string refreshToken = _currentUser.RefreshToken;
        
        if (!string.IsNullOrEmpty(refreshToken))
        {
            // Revoke the refresh token in database
            var dbToken = await _refreshTokenRepository.GetByToken(refreshToken);
            if (dbToken != null)
            {
                dbToken.Revoke();
                await _refreshTokenRepository.SaveChangesAsync(cancellationToken);
            }
        }

        // Remove refresh token cookie
        _tokenService.RemoveRefreshTokenFromCookies();
        
        return true;
    }
}
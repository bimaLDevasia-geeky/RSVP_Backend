using System;
using RSVP.Domain.Entities;

namespace RSVP.Application.Interfaces;

public interface ITokenService
{
    public string GenerateAccessToken(int userId, string userEmail,string Role);
    public RefreshToken GenerateRefreshToken(int userId);
    public void SetRefreshTokenInCookies(RefreshToken refreshToken);
}

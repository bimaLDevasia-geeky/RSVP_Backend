using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using RSVP.Application.Interfaces;
using RSVP.Domain.Entities;

namespace RSVP.Infrastructure.Service;

public class TokenService:ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TokenService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }

    public string GenerateAccessToken(int userId, string userEmail, string Role)
    {
        string? secretKey = _configuration["JwtSettings:SecretKey"];
        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
        string? issuer = _configuration["JwtSettings:Issuer"];
         string? audience = _configuration["JwtSettings:Audience"];
        int expiryInMinutes = Convert.ToInt32(_configuration["JwtSettings:ExpiryInMinutes"]);

         SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var Claims = new List<Claim>()
        {
          new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
          new Claim(ClaimTypes.Email, userEmail),  
          new Claim(ClaimTypes.Role, Role)
        };

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: Claims,
            expires: DateTime.UtcNow.AddMinutes(expiryInMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public RefreshToken GenerateRefreshToken(int userId)
    {

        int expiresInDays = Convert.ToInt32(_configuration["JwtSettings:RefreshTokenExpiryInDays"]);
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);

            return new RefreshToken
            (
                userId : userId,
                token : Convert.ToBase64String(randomNumber),
                expiresAt : DateTime.UtcNow.AddDays(expiresInDays)
            );
        }
    }

    public void SetRefreshTokenInCookies(RefreshToken refreshToken)
    {
        
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(refreshToken.ExpiresAt.Day)
        };
        _httpContextAccessor.HttpContext.Response.Cookies.Append("RefreshToken", refreshToken.Token, cookieOptions);
    }
}

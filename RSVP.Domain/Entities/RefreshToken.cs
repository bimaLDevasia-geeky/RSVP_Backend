using System;

namespace RSVP.Domain.Entities;

public class RefreshToken
{
    public int Id { get; private set; }
    public int UserId { get; private set; }
    public string Token { get; private set; }=null!;
    public DateTime ExpiresAt { get; private set; }

    public User User { get; private set; }=null!;

    public Boolean IsRevoked { get; private set; } = false;

    public RefreshToken(int userId, string token, DateTime expiresAt)
    {
        UserId = userId;
        Token = token;
        ExpiresAt = expiresAt;
    }

    public void Revoke()
    {
        IsRevoked = true;
    }

    
}

using System;

namespace RSVP.Application.Interfaces;

public interface ICurrentUser
{
    int UserId { get; }

    string Role { get;  }

    string Email { get; }

    string RefreshToken { get; }
    
}

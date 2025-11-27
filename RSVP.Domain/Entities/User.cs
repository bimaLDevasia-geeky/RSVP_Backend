using System;
using RSVP.Domain.Enums;

namespace RSVP.Domain.Entities;

public class User
{
    public int Id { get; private set; }
    public string Name { get; private set; }=null!;
    public string Email { get; private set; }=null!;
    public string HashedPassword { get; private set; } =null!;
    public UserStatus Status { get; private set; }
    public UserRole Role { get; private set; } 
    public ICollection<Event> CreatedEvents { get; private set; } = new List<Event>();
    public ICollection<Request> Requests { get; private set; } = new List<Request>();
    public ICollection<Notification> Notifications { get; private set; } = new List<Notification>();
    public ICollection<RefreshToken> RefreshTokens { get; private set; } = new List<RefreshToken>();
    
    public User(string name, string email, string hashedPassword)
    {
        Name = name;
        Email = email;
        HashedPassword = hashedPassword;
        Status = UserStatus.Active;
        Role = UserRole.User;
    }

    public void UpdateUser(string? name, string? email)
    {
        if (name != null)
        {
            Name = name;
        }
        if (email != null)
        {
            Email = email;
        }
    }

    public void ChangeStatus(UserStatus newStatus)
    {
        Status = newStatus;
    }

    public void ChangeRole(UserRole newRole)
    {
        Role = newRole;
    }
}

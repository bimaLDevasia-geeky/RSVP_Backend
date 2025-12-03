using System;
using Microsoft.EntityFrameworkCore;
using RSVP.Domain.Entities;

namespace RSVP.Application.Interfaces;

public interface IRsvpDbContext
{
    public DbSet<Event> Events { get;  }
    public DbSet<Attendie> Attendies { get; set; }
    public DbSet<Media> Media { get; set; }
    public DbSet<User> Users { get;  }
    public DbSet<Notification> Notifications { get;  }
    public DbSet<Request> Requests { get;  }
    public DbSet<RefreshToken> RefreshTokens { get;  }
    public DbSet<ChatMessage> ChatMessages { get;  }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
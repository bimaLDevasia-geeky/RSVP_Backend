using System;
using Microsoft.EntityFrameworkCore;
using RSVP.Application.Interfaces;
using RSVP.Domain.Entities;

namespace RSVP.Infrastructure.Persistence;

public class RsvpDbContext:DbContext, IRsvpDbContext
{
    public RsvpDbContext(DbContextOptions<RsvpDbContext> options):base(options)
    {

    }
    public DbSet<Event> Events { get; set; } = null!;
    public DbSet<Attendie> Attendies { get; set; } = null!;
    public DbSet<Media> Media { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Notification> Notifications { get; set; } = null!;
    public DbSet<Request> Requests { get; set; } = null!;
    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
    public DbSet<ChatMessage> ChatMessages { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RsvpDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

}

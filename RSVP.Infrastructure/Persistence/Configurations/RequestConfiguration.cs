using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RSVP.Domain.Entities;

namespace RSVP.Infrastructure.Persistence.Configurations;

public class RequestConfiguration:IEntityTypeConfiguration<Request>
{
    public void Configure(EntityTypeBuilder<Request> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.EventId)
            .IsRequired();

        builder.Property(r => r.UserId)
            .IsRequired();

        builder.Property(r => r.RequestedAt)
            .IsRequired();

        builder.Property(r => r.Status)
            .IsRequired();

        builder.HasOne(r => r.Event)
            .WithMany(e => e.Requests)
            .HasForeignKey(r => r.EventId)
            .OnDelete(DeleteBehavior.Cascade);

     
        builder.HasOne(r => r.User)
            .WithMany(u => u.Requests)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}

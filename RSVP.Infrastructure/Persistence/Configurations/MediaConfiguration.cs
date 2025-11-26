using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RSVP.Domain.Entities;

namespace RSVP.Infrastructure.Persistence.Configurations;

public class MediaConfiguration : IEntityTypeConfiguration<Media>
{
    public void Configure(EntityTypeBuilder<Media> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.EventId)
            .IsRequired();

        builder.Property(m => m.Url)
            .IsRequired()
            .HasMaxLength(1000);

     
        builder.HasOne(m => m.Event)
            .WithMany(e => e.Media)
            .HasForeignKey(m => m.EventId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

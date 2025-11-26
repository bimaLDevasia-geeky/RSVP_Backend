using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RSVP.Domain.Entities;

namespace RSVP.Infrastructure.Persistence.Configurations;

public class AttendiesConfiguration:IEntityTypeConfiguration<Attendie>
{
    public void Configure(EntityTypeBuilder<Attendie> builder)
    {
        

        builder.HasKey(a => a.Id);


        builder.HasOne(a=> a.Event)
            .WithMany(e=>e.Attendies)
            .HasForeignKey(a => a.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(a => a.EventId)
            .IsRequired()
;

        builder.Property(a => a.UserId)
            .IsRequired(false);

        builder.Property(a => a.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(a => a.Role)
            .IsRequired();

        builder.Property(a => a.Status)
            .IsRequired();
    }
}

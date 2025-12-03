using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RSVP.Domain.Entities;

namespace RSVP.Infrastructure.Persistence.Configurations;

public class ChatMessageConfiguration:IEntityTypeConfiguration<ChatMessage>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.ChatMessage> builder)
    {
        builder.HasKey(cm => cm.Id);

        builder.Property(cm => cm.UserName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(cm => cm.Message)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(cm => cm.Timestamp)
            .IsRequired();

        builder.HasOne(cm => cm.Event)
            .WithMany(e => e.Chats)
            .HasForeignKey(cm => cm.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(cm => cm.User)
            .WithMany(u => u.Chats)
            .HasForeignKey(cm => cm.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(cm => new { cm.EventId, cm.Timestamp });
    }
}


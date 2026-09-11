using Chat.Domain.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Persistence.Context.Configuration;

public class ConversationConfiguration : IEntityTypeConfiguration<Conversation>
{
 

    public void Configure(EntityTypeBuilder<Conversation> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.IssueId).IsRequired();
        builder.Property(c=>c.Status).IsRequired();

        builder.HasMany(p => p.Participants)
        .WithOne(c => c.Conversation)
        .OnDelete(DeleteBehavior.Cascade);
 
        builder.HasMany(p => p.Messages)
            .WithOne(c => c.Conversation)
            .OnDelete(DeleteBehavior.Cascade);


    }
}
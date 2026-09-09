using Chat.Domain.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Persistence.Context.Configuration
{
    public class MessageAttachmentConfiguration : IEntityTypeConfiguration<MessageAttachment>
    {
        public void Configure(EntityTypeBuilder<MessageAttachment> builder)
        {
              builder.HasKey(x => x.Id);
              builder.Property(x => x.FileName).IsRequired().HasMaxLength(30);
            builder.Property(x => x.FileUrl).IsRequired().HasMaxLength(255);
            builder.Property(x => x.ContentType).IsRequired().HasMaxLength(255);
            builder.Property(x => x.FileSize).IsRequired().HasMaxLength(255);
            builder.HasOne(x=>x.Message)
                .WithMany(x=>x.Attachments)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

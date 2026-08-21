using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Issue.Domain.Entities.Issue;

namespace Issue.Persistence.Context.Configuration
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Text).HasMaxLength(2000);
            builder.Property(c => c.VoiceUrl).HasMaxLength(500);
            builder.Property(c => c.UserId).IsRequired();
        }
    }
}
using Issue.Domain.Entities.Issue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Issue.Persistence.Context.Configuration;

    public class IssueSharedConfguration : IEntityTypeConfiguration<IssueShared>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<IssueShared> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.IssueId)
                .IsRequired();
            builder.Property(x => x.UserId)
                .IsRequired();

            builder.HasOne(x => x.Issue)
                .WithMany(x => x.Shares)
                .HasForeignKey(x => x.IssueId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

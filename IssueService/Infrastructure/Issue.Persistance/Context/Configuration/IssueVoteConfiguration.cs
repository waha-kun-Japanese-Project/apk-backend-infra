using Issue.Domain.Entities.Issue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Issue.Persistence.Context.Configuration;

public class IssueVoteConfiguration : IEntityTypeConfiguration<IssueVote>
{
    public void Configure(EntityTypeBuilder<IssueVote> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.IssueId)
            .IsRequired();

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.HasOne(x => x.Issue)
            .WithMany(x => x.Votes)
            .HasForeignKey(x => x.IssueId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new
        {
            x.IssueId,
            x.UserId
        })
        .IsUnique();
    }
}
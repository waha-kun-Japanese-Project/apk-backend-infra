using Issue.Domain.Entities.Issue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Persistence.Context.Configuration
{
    public class IssueConfiguration : IEntityTypeConfiguration<Issue.Domain.Entities.Issue.Issue>
    {
        public void Configure(EntityTypeBuilder<Issue.Domain.Entities.Issue.Issue> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.Title).IsRequired().HasMaxLength(200);
            builder.Property(i => i.Description).HasMaxLength(2000);
            builder.Property(i => i.Type).IsRequired().HasMaxLength(100);
            builder.Property(i => i.Status).IsRequired();
            builder.Property(i => i.Priority).IsRequired();
            builder.Property(i => i.ReporterId).IsRequired();

            builder.HasOne(i => i.RepairSchedule)
                .WithOne(rs => rs.Issue)
                .HasForeignKey<RepairSchedule>(rs => rs.IssueId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(i => i.Feedback)
                .WithOne(f => f.Issue)
                .HasForeignKey<IssueFeedback>(f => f.IssueId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(i => i.ExpertReviews)
                .WithOne(er => er.Issue)
                .HasForeignKey(er => er.IssueId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(i => i.ResolutionActions)
                .WithOne(ra => ra.Issue)
                .HasForeignKey(ra => ra.IssueId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(i => i.StatusHistory)
                .WithOne(sh => sh.Issue)
                .HasForeignKey(sh => sh.IssueId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(i => i.Comments)
                .WithOne(c => c.Issue)
                .HasForeignKey(c => c.IssueId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(i => i.Notifications)
                .WithOne(n => n.RelatedIssue)
                .HasForeignKey(n => n.RelatedIssueId)
                .OnDelete(DeleteBehavior.SetNull);
            builder.HasMany(i => i.Votes)
                .WithOne(v => v.Issue)
                .HasForeignKey(v => v.IssueId)
                .OnDelete(DeleteBehavior.Cascade);
            
          
           
            builder.HasMany(i => i.IssueAttachments)
                .WithOne(ia => ia.Issue)
                .HasForeignKey(ia => ia.IssueId)
                .OnDelete(DeleteBehavior.Cascade);
          

        }
    }
}
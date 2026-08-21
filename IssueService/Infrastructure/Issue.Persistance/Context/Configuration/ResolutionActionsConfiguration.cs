using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Issue.Domain.Entities.Issue;

namespace Issue.Persistence.Context.Configuration
{
    public class ResolutionActionsConfiguration : IEntityTypeConfiguration<ResolutionActions>
    {
        public void Configure(EntityTypeBuilder<ResolutionActions> builder)
        {
            builder.HasKey(ra => ra.Id);
            builder.Property(ra => ra.ActionType).IsRequired().HasMaxLength(200);
            builder.Property(ra => ra.Notes).HasMaxLength(2000);
        }
    }
}

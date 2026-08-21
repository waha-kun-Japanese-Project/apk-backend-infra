using Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Persistence.Context.Configrations
{
    internal class RefreshTokenConfig: IEntityTypeConfiguration<RefreshToken>
    {
      
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
           builder.Property(rt => rt.Token)
                .IsRequired()
                .HasMaxLength(255);
            builder.Property(rt => rt.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Property(rt => rt.ExpiresAt)
                .IsRequired();
            builder.HasOne(rt => rt.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

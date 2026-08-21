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
    public  class RoleConfig:IEntityTypeConfiguration<AppRole>
    {
        public void Configure(EntityTypeBuilder<AppRole> builder)
        {
            builder.Property(r => r.Description)
                .HasMaxLength(255);
            builder.Property(r => r.Name)
                .IsRequired();
            builder.Property(x => x.CreatedAt)
                 .HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.HasMany(r => r.RolePermissions)
                .WithOne(rp => rp.Role)
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.Cascade);



        }
    }
}

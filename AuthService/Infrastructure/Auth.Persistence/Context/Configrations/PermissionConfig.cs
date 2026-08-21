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
    public class PermissionConfig : IEntityTypeConfiguration<Permission>

    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
                builder.Property(p => p.Name)
                    .IsRequired()
                    .HasMaxLength(100);
    
                builder.Property(p => p.Description)
                    .HasMaxLength(255);
    
                builder.Property(x => x.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
    
                builder.HasMany(p => p.RolePermissions)
                    .WithOne(rp => rp.Permission)
                    .HasForeignKey(rp => rp.PermissionId)
                    .OnDelete(DeleteBehavior.Cascade);
    
                builder.HasMany(p => p.UserPermissions)
                    .WithOne(up => up.Permission)
                    .HasForeignKey(up => up.PermissionId)
                    .OnDelete(DeleteBehavior.Cascade);



        }
    }
}

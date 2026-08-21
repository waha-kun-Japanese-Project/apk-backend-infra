using Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Persistence.Context.Configrations
{
    internal class UserConfig : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(u => u.UserName)
               .IsRequired()
               .HasMaxLength(100);
            builder.Property(u => u.Email).IsRequired(false)
                .HasMaxLength(100);


            builder.Property(u => u.pictures)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(x => x.Status)
                 .HasMaxLength(20)
                 .HasDefaultValue(UserStatus.Approved)
                 .IsRequired();

            builder.Property(x => x.CreatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(x => x.PhoneNumber)
                .IsRequired()
                .HasMaxLength(16);

            builder.HasIndex(x => x.PhoneNumber)
               .IsUnique(); 

            builder.HasMany(u => u.UserPermissions)
                .WithOne(up => up.User)
                .HasForeignKey(up => up.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(u => u.Address)
                .WithOne(a => a.User)
                .HasForeignKey<Address>(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);
           
        }
    }
}

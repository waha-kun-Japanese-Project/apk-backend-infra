using Chat.Domain.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Persistence.Context.Configuration;

public class QuickReplyConfiguration : IEntityTypeConfiguration<QuickReply>
{

    public void Configure(EntityTypeBuilder<QuickReply> builder)
    {
        builder.HasKey(qr => qr.Id);
        builder.Property( qr => qr.Text).IsRequired().HasMaxLength(2000);
        
    }
}

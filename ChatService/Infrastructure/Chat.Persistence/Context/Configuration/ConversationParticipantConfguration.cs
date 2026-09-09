using Chat.Domain.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Persistence.Context.Configuration
{
    public class ConversationParticipantConfguration : IEntityTypeConfiguration<ConversationParticipant>
    {
        public void Configure(EntityTypeBuilder<ConversationParticipant> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Role).IsRequired();
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.ConversationId).IsRequired();
        }
    }
}

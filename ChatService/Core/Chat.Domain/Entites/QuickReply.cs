using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Domain.Entites
{
    public class QuickReply : BaseEntity<Guid>
    {
        public string Text { get; set; } = null!;

        public ParticipantRole? TargetRole { get; set; }

        public bool IsActive { get; set; }

        public int DisplayOrder { get; set; }
    }
}

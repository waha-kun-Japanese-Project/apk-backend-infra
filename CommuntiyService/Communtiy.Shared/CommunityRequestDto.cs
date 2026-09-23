using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communtiy.Shared
{
    public class CommunityRequestDto
    {
        public Guid IssueId { get; set; }
        public Guid UserId { get; set; }
        public string Text { get; set; } = null!;
        public string VoiceUrl { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
    }
}

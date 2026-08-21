using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Domain.Entities.Issue
{
    public class Comment: BaseEntity<Guid>
    {
        public string? Text { get; set; }
        public string? VoiceUrl { get; set; }

        public Guid IssueId { get; set; }
        public Issue Issue { get; set; } = null!;

        public Guid UserId { get; set; }
    }
}

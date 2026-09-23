using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communtiy.Shared.Clients
{
    public class ModerationResponseDto
    {
        public string? CommentId { get; set; }
        public string Text { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;   // Relevant | Spam | Offensive | Irrelevant
        public double Confidence { get; set; }
        public bool IsFlagged { get; set; }
        public double ProcessingTimeMs { get; set; }
        public DateTime Timestamp { get; set; }
    }
}

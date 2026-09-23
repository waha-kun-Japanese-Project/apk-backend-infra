using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communtiy.Shared.Clients
{
    public class ModerationRequestDto
    {
        public string? CommentId { get; set; }
        public string Text { get; set; } = string.Empty;
    }
}

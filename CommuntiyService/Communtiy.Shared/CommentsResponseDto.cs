using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communtiy.Shared
{
    public class CommentsResponseDto
    {
        public IEnumerable<CommentResponseDto> Comments { get; set; } = null!;
        public long Count { get; set; }
    }
}

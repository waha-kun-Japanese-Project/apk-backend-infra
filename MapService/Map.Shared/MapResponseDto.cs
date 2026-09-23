

using Map.Domain.Entities.ISSUE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Map.Shared
{
    public class MapResponseDto
    {
        public Guid IssueId { get; set; }
       
        public string Longitde { get; set; }
        public  string Latitude { get; set; }
        public string? PhotoUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public string priority { get; set; }
        public string Status { get; set; }
        public string title { get; set; }

    }
       
}

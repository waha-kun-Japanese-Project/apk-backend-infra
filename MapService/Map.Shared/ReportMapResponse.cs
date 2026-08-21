using Issue.Domain.Entities.Issue;
using Map.Domain.Entities.REPORT;
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
        public Guid ReportId { get; set; }
        public string Longitde { get; set; }
        public  string Latitude { get; set; }
        public IssuePriority priority { get; set; }

    }
       
}

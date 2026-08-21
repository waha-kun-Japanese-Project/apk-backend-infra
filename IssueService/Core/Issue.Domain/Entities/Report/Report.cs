
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Issue.Domain.Entities.Report
{
    public class Report : BaseEntity<Guid>
    {
        public string? Description { get; set; }
        public ReportStatus Status { get; set; } = ReportStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public Guid ReporterId { get; set; }  // user from auth context  frammmer



        public AiAnalysis? Analysis { get; set; }

        public ICollection<ReportAttachment> Attachments { get; set; } = new List<ReportAttachment>();

        
        public GPSLocation? Location { get; set; }
        public Guid? LocationId { get; set; }


        }

}

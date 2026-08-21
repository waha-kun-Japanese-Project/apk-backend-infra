using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Domain.Entities.Report
{
    public class ReportAttachment : BaseEntity<Guid>
    {
        public ReportAttachmentType Type { get; set; }
        public string Url { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Guid ReportId { get; set; }
        public Report Report { get; set; } = null!;
    }
}

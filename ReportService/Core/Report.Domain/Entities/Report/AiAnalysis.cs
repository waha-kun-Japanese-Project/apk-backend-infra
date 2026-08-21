using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Domain.Entities.Report
{
    public class AiAnalysis : BaseEntity<Guid>
    {
        public Guid ReportId { get; set; }
        public Report Report { get; set; } = null!;

        public string ProblemName { get; set; } = null!;
        public string? ProblemArabic { get; set; }

        public double Confidence { get; set; }

        public SeverityLevel Severity { get; set; }

        public string Recommendation { get; set; } = string.Empty;

        public string? Explanation { get; set; }

        public List<string> RepairSteps { get; set; } = new();

        public string ModelVersion { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}



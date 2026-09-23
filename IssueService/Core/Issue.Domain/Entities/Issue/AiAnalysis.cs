using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Domain.Entities.Issue
{
    public class AiAnalysis : BaseEntity<Guid>
    {

        public string ProblemName { get; set; } = null!;
        public string? ProblemArabic { get; set; }

        public double Confidence { get; set; }

        public string Severity { get; set; } = null!;

        public string Recommendation { get; set; } = string.Empty;

        public string? Explanation { get; set; }

        public List<string> RepairSteps { get; set; } = new();

        public string ModelVersion { get; set; } = string.Empty;

    }
}



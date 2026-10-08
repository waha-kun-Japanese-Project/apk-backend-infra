using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Shared.DTOS
{
    public record AiAnalysisSummary
    {
        public string ProblemName { get; init; } = string.Empty;
        public string? ProblemArabic { get; init; }
        public double Confidence { get; init; }
        public string Severity { get; init; } = string.Empty;
        public string Recommendation { get; init; } = string.Empty;
        public string? Explanation { get; init; }
        public List<string> RepairSteps { get; init; } = new();
    }
}

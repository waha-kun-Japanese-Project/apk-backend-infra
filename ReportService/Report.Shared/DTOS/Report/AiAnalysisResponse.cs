using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Shared.DTOS.Report
{

    public record AiAnalysisResponse
    {
        public string FilePath { get; init; } = string.Empty;
        public string ProblemName { get; init; } = string.Empty;
        public string? ProblemArabic { get; init; }
        public double Confidence { get; init; }
        public string Severity { get; init; } = string.Empty;
        public string Recommendation { get; init; } = string.Empty;
        public string? Explanation { get; init; }
        public List<string> RepairSteps { get; init; } = new();
    }
    //public class SeverityDto
    //{
    //    public string Level { get; set; }
    //    public string urgency { get; set; }
    //    public string recommended_action { get; set; }
    //    // Add other fields returned by FastAPI inside the severity object
    //}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Shared.DTOS.Report
{

    public record AiAnalysisResponse(
        string FilePath,
        string ProblemName,
        string? ProblemArabic,
        double Confidence,
        string Severity,
        string Recommendation,
        string? Explanation,
        List<string> RepairSteps
       
    );
    //public class SeverityDto
    //{
    //    public string Level { get; set; }
    //    public string urgency { get; set; }
    //    public string recommended_action { get; set; }
    //    // Add other fields returned by FastAPI inside the severity object
    //}
}

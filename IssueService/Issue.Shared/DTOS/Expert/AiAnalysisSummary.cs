using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Shared.DTOS
{
    public record AiAnalysisSummary(
         string ProblemName,
         string? ProblemArabic,
         double Confidence,
         string Severity,
         string Recommendation,
         string? Explanation,
         List<string> RepairSteps
     );
}

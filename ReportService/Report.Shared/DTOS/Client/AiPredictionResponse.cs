using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Report.Shared.DTOS.Client
{
     public record AiPredictionResponse(
        [property: JsonPropertyName("status")] string Status,
        [property: JsonPropertyName("problem")] string? Problem,
        [property: JsonPropertyName("problem_code")] string? ProblemCode,
        [property: JsonPropertyName("confidence")] string? Confidence,
        [property: JsonPropertyName("severity")] string? Severity,
        [property: JsonPropertyName("recommendation")] string? Recommendation,
        [property: JsonPropertyName("explanation")] string? Explanation,
        [property: JsonPropertyName("repair_steps")] List<string>? RepairSteps,
        [property: JsonPropertyName("message")] string? Message,
        [property: JsonPropertyName("suggestion")] string? Suggestion,
        [property: JsonPropertyName("timestamp")] string Timestamp
    )
    
    {
    }
}

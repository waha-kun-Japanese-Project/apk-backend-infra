using Report.Domain.Entities.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Service.Mapping
{
    public static class AiAnalysisMapper
    {

        private static readonly Dictionary<string, SeverityLevel> ArabicSeverityMap = new()
        {
            ["غير معروف"] = SeverityLevel.Unknown,

            ["ضئيلة"] = SeverityLevel.Negligible,

            ["بسيطة جدا"] = SeverityLevel.VeryMinor,
            ["بسيطة جداً"] = SeverityLevel.VeryMinor,

            ["بسيطة"] = SeverityLevel.Minor,

            ["منخفضة"] = SeverityLevel.Low,

            ["متوسطة"] = SeverityLevel.Medium,

            ["عالية"] = SeverityLevel.High,

            ["عالية جدا"] = SeverityLevel.VeryHigh,
            ["عالية جداً"] = SeverityLevel.VeryHigh,

            ["حرجة"] = SeverityLevel.Critical,

            ["حرجة جدا"] = SeverityLevel.VeryCritical,
            ["حرجة جداً"] = SeverityLevel.VeryCritical
        };


        public static SeverityLevel MapSeverity(string? severity)
        {
            if (string.IsNullOrWhiteSpace(severity))
                return SeverityLevel.Unknown;

            severity = severity.Trim();

            if (Enum.TryParse(severity, true, out SeverityLevel parsed))
                return parsed;

            return ArabicSeverityMap.TryGetValue(severity, out var value)
                ? value
                : SeverityLevel.Unknown;
        }
    
    public static double ParseConfidence(string? confidence)
        {
            if (string.IsNullOrWhiteSpace(confidence)) return 0;
            return double.TryParse(confidence.TrimEnd('%'), System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture, out var value) ? value : 0;
        }

    }
   }

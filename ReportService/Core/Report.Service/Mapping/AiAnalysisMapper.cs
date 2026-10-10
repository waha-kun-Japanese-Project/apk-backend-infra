using System.Globalization;

namespace Report.Service.Mapping
{
    public static class AiAnalysisMapper
    {
        public static double ParseConfidence(string? confidence)
        {
            if (string.IsNullOrWhiteSpace(confidence))
                return 0;

            var trimmed = confidence.Trim().TrimEnd('%');
            return double.TryParse(trimmed, NumberStyles.Any, CultureInfo.InvariantCulture, out var value)
                ? value
                : 0;
        }

    }
}
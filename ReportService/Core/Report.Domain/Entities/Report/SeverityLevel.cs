using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Domain.Entities.Report
{
    public  enum SeverityLevel
    {
        Unknown = 0,
        Negligible = 1,
        VeryMinor = 2,
        Minor = 3,
        Low = 4,
        Medium = 5,
        High = 6,
        VeryHigh = 7,
        Critical = 8,
        VeryCritical = 9
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Map.Domain.Entities.REPORT
{
    public class GpsLocation
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }

        public Report Report { get; set; } = null!;
    }
}

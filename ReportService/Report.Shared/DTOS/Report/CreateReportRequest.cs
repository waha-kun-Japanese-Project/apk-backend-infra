using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Shared.DTOS.Report
{
    public  record  CreateReportRequest(IFormFile photo, string? Description,
        string? Latitude,
        string? Longitude)
    {
    }
}

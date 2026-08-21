using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Shared.DTOS.Report
{
    public record  ReportAttachmentResponse(Guid Id,
        string Type,
        string Url,
        DateTime CreatedAt)
    {
    }
}

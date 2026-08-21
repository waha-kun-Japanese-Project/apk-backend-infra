using Report.Shared.DTOS.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Shared.DTOS.Report
{
    public  record  ReportDetailsResponse(
     Guid Id,
    string? Description,
    string Status,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    Guid ReporterId,
    double? Latitude,
    double? Longitude,
    List<ReportAttachmentResponse> Attachments,
    AiAnalysisResponse? Analysis
     )
    {
    }
}

using Microsoft.AspNetCore.Http;
using Report.Domain.Entities.Issue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Shared.DTOS.Report
{
    public  record  CreateIssueRequest(string Title,
    string? Description,
    Guid ReporterId,
    IssuePriority Priority,
  string Longitude,
  string Latitude,
    Guid IssueAttachmentId

        )
    {
    }
}




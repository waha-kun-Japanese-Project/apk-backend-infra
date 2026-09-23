using Microsoft.AspNetCore.Http;
using Report.Shared.DTOS.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.ServiceAbstraction
{
    public  interface IIssueService
    {
        Task<CreateIssueResponse> CreateIssueAsync(CreateIssueRequest request,CancellationToken cancellationToken=default );

        //Task<ReportDetailsResponse> GetIssueByIdAsync(Guid id, CancellationToken cancellationToken = default);

        //Task<IEnumerable<ReportDetailsResponse>> GetAllIssuesAsync(CancellationToken cancellationToken = default);

        //Task<IEnumerable<ReportDetailsResponse>> GetMyIssuesAsync(CancellationToken cancellationToken = default);

        Task<AiAnalysisResponse> AnalyzeIssueAsync(IFormFile photo, CancellationToken cancellationToken = default);

        Task DeleteIssueAsync(Guid id, CancellationToken cancellationToken = default);
    }
}

using Report.Shared.DTOS.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.ServiceAbstraction
{
    public  interface IReportService
    {
        Task<CreateReportResponse> CreateReportAsync(CreateReportRequest request,CancellationToken cancellationToken=default );

        Task<ReportDetailsResponse> GetReportByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<IEnumerable<ReportDetailsResponse>> GetAllReportsAsync(CancellationToken cancellationToken = default);

        Task<IEnumerable<ReportDetailsResponse>> GetMyReportsAsync(CancellationToken cancellationToken = default);

        Task<ReportDetailsResponse> AnalyzeReportAsync(Guid id, CancellationToken cancellationToken = default);

        Task DeleteReportAsync(Guid id, CancellationToken cancellationToken = default);
    }
}

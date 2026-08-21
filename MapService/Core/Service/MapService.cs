using Map.Domain.Contarcts;
using Map.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Map.Service
{
    public class MapService(IIsuueRepo issueRepo , IReportREpo reportRepo) : Map.ServiceAbsraction.IMapSerevice
    {
        public async Task<MapResponseDto> SearchForIssueInMapAsync(Guid IssueId , CancellationToken cancellationToken)
        {
            var issue = await issueRepo.GetByIdAsync(IssueId);

            if (issue is null)
                throw new KeyNotFoundException("Issue not found.");

            var report = await reportRepo.GetByIdAsync(issue.ReportId);

            if (report is null ||report.Location is null)
                throw new KeyNotFoundException("Report location not found.");

            return new MapResponseDto
            {
                IssueId = issue.Id,
                priority = issue.Priority,
                ReportId = issue.ReportId,
                Latitude = report.Location.Latitude,
                Longitde = report.Location.Longitude
            };
        }

        public async Task<IEnumerable<MapResponseDto>> ShowIssueInMapAsync(CancellationToken cancellationToken)
        {
            var issues = await issueRepo.GetAllAsync();
            var result = new List<MapResponseDto>();
            foreach (var issue in issues)
            {
                var report = await reportRepo.GetByIdAsync(issue.ReportId);

                if (report is null || report.Location is null)

                  throw new KeyNotFoundException("Report location not found.");

                result.Add(new MapResponseDto
                {
                    IssueId = issue.Id,
                    priority = issue.Priority,
                    ReportId = issue.ReportId,
                    Latitude = report.Location.Latitude,
                    Longitde = report.Location.Longitude
                });
            }

            return result;
        }
    }
}

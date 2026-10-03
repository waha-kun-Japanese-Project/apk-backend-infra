using AutoMapper;
using Microsoft.AspNetCore.Http;
using Refit;
using Report.Client.AbstructServices;
using Report.Domain.Contracts;
using Report.Domain.Entities.Report;
using Report.ServiceAbstraction;
using Report.Shared.DTOS.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Report.Service.Services
{
    public class ReportService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor,
        IStorageClient storageClient,
        IAiVisionClient aiVisionClient) : IReportService
    {
        public async Task<CreateReportResponse> CreateReportAsync(CreateReportRequest request, CancellationToken cancellationToken = default)
        {
            var reporterId = GetLoggedInUserId();

            var uploadResult = await storageClient.UploadAsync(
                new StreamPart(request.photo.OpenReadStream(), request.photo.FileName, request.photo.ContentType),
                "reportimage");

            var report = new Report.Domain.Entities.Report.Report
            {
                Description = request.Description,
                ReporterId = reporterId,
                Status = ReportStatus.Pending
            };

            if (!string.IsNullOrWhiteSpace(request.Latitude) && !string.IsNullOrWhiteSpace(request.Longitude))
            {
                report.Location = new GPSLocation
                {
                    Latitude = request.Latitude,
                    Longitude = request.Longitude
                };
            }

            report.Attachments.Add(new ReportAttachment
            {
                Type = ReportAttachmentType.Photo,
                Url = uploadResult.filePath
            });

            await unitOfWork.reportRepo.AddAsync(report);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return mapper.Map<CreateReportResponse>(report);
        }

        public async Task<ReportDetailsResponse> AnalyzeReportAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var report = await unitOfWork.reportRepo.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("Report not found.");

            var photoUrl = report.Attachments
                .Where(a => a.Type == ReportAttachmentType.Photo)
                .Select(a => a.Url)
                .FirstOrDefault()
                ?? throw new InvalidOperationException("Report has no photo to analyze.");

            await using var stream = await storageClient.DownloadAsync(photoUrl);

            var prediction = await aiVisionClient.PredictAsync(
    new StreamPart(stream, Path.GetFileName(photoUrl), "image/jpeg"));

            if (prediction.Status != "success")
            {
                throw new InvalidOperationException(
                    prediction.Message ?? "The vision service couldn't analyze the image.");
            }

            report.Analysis = new AiAnalysis
            {
                ProblemName = prediction.ProblemCode,
                ProblemArabic = prediction.Problem,
                Confidence = Mapping.AiAnalysisMapper.ParseConfidence(prediction.Confidence),
                Severity = Mapping.AiAnalysisMapper.ParseSeverity(prediction.Severity),
                Recommendation = prediction.Recommendation,
                Explanation = prediction.Explanation,
                RepairSteps = prediction.RepairSteps ?? new List<string>()
            };
            report.Status = ReportStatus.Analyzed;
            report.UpdatedAt = DateTime.UtcNow;

            await unitOfWork.reportRepo.UpdateAsync(report);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return mapper.Map<ReportDetailsResponse>(report);
        }

        public async Task<ReportDetailsResponse> GetReportByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var report = await unitOfWork.reportRepo.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("Report not found.");

            return mapper.Map<ReportDetailsResponse>(report);
        }

        public async Task<IEnumerable<ReportDetailsResponse>> GetAllReportsAsync(CancellationToken cancellationToken = default)
        {
            var reports = await unitOfWork.reportRepo.GetAllAsync();
            return mapper.Map<IEnumerable<ReportDetailsResponse>>(reports);
        }

        public async Task<IEnumerable<ReportDetailsResponse>> GetMyReportsAsync(CancellationToken cancellationToken = default)
        {
            var reporterId = GetLoggedInUserId();
            var reports = await unitOfWork.reportRepo.GetByReporterIdAsync(reporterId);
            return mapper.Map<IEnumerable<ReportDetailsResponse>>(reports);
        }

        public async Task DeleteReportAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var report = await unitOfWork.reportRepo.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("Report not found.");

            var deleteTasks = report.Attachments.Select(a => storageClient.DeleteAsync(a.Url));
            await Task.WhenAll(deleteTasks);

            await unitOfWork.reportRepo.DeleteAsync(id);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        private Guid GetLoggedInUserId()
        {
            var user = httpContextAccessor.HttpContext?.User;
            if (user is null || !user.Identity!.IsAuthenticated)
            {
                throw new UnauthorizedAccessException("User not authenticated.");
            }

            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var parsedId))
            {
                throw new UnauthorizedAccessException("User Id not found in token.");
            }

            return parsedId;
        }
    }
}
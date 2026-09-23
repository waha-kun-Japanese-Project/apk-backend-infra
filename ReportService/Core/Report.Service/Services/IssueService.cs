using AutoMapper;
using Microsoft.AspNetCore.Http;
using Refit;
using Report.Client.AbstructServices;
using Report.Domain.Contracts;
using Report.Domain.Entities.Issue;
using Report.Service.Mapping;
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
    public class IssueService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor,
        IStorageClient storageClient,
        IAiVisionClient aiVisionClient) : IIssueService
    {
        public async Task<AiAnalysisResponse> AnalyzeIssueAsync(IFormFile photo, CancellationToken cancellationToken = default)
        {
            if (photo is null)
            {
                throw new ArgumentNullException(nameof(photo), "Photo is required for analysis.");
            }

            await using var stream = photo.OpenReadStream();
            var uploadResult = await storageClient.UploadAsync(
                new StreamPart(stream, photo.FileName, photo.ContentType),"reportimage");

            await using var analysisStream = photo.OpenReadStream();

            var prediction = await aiVisionClient.PredictAsync(
                new StreamPart(
                    analysisStream,
                    photo.FileName,
                    photo.ContentType));

            if (prediction.Status != "success")
            {
                await storageClient.DeleteAsync(uploadResult.filePath);

                throw new InvalidOperationException(
                    prediction.Message ??
                    "The vision service couldn't analyze the image.");
            }

            return new AiAnalysisResponse(
                FilePath: uploadResult.filePath,
                ProblemName: prediction.ProblemCode,
                ProblemArabic: prediction.Problem,
                Confidence: AiAnalysisMapper.ParseConfidence(prediction.Confidence),
                Severity: prediction.Severity,
                Recommendation: prediction.Recommendation,
                Explanation: prediction.Explanation,
                RepairSteps: prediction.RepairSteps ?? new List<string>()
            );
        }

        public async Task<CreateIssueResponse> CreateIssueAsync(CreateIssueRequest request, CancellationToken cancellationToken = default)
        {
            var priority = GetPriority(request.AiAnalysisResponse.Severity);
            if (priority is IssuePriority.Low or IssuePriority.Unknown)
            {
                await storageClient.DeleteAsync(request.AiAnalysisResponse.FilePath);

                throw new InvalidOperationException(
                    "Issue was not created because the detected problem's priority is too low to report.");
            }

            var reporterId = GetLoggedInUserId();

            var issue = mapper.Map<Issue>(request);

            issue.ReporterId = reporterId;

            issue.IssueAttachments.Add(new IssueAttachment
            {
                Type = IssueAttachmentType.Photo,
                Url = request.AiAnalysisResponse.FilePath
            });

            issue.AiAnalyses.Add(
                mapper.Map<AiAnalysis>(request.AiAnalysisResponse)
            );

            issue.Priority = priority;
            issue.Title = request.AiAnalysisResponse.ProblemArabic;
            issue.Description=request.AiAnalysisResponse.Explanation;
            issue.Status = IssueStatus.Diagnosed;
            await unitOfWork.issueRepo.AddAsync(issue);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return mapper.Map<CreateIssueResponse>(issue);
        }

        public async Task DeleteIssueAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var issue = await unitOfWork.issueRepo.GetByIdAsync(id)
               ?? throw new KeyNotFoundException("Report not found.");
            var deleteTasks = issue.IssueAttachments
     .Select(a => storageClient.DeleteAsync(a.Url));

            await Task.WhenAll(deleteTasks);

            await unitOfWork.issueRepo.DeleteAsync(id);
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

        private IssuePriority GetPriority(string severity) {

            return severity?.Trim() switch
            {
                // Critical
                "حرجة جداً" => IssuePriority.Critical,
                "حرجة" => IssuePriority.Critical,
                "عالية جداً" => IssuePriority.Critical,

                // High
                "عالية" => IssuePriority.High,
                "متوسطة" => IssuePriority.Medium,
                "منخفضة" => IssuePriority.Medium,

                // Low
                "بسيطة" => IssuePriority.Low,
                "بسيطة جداً" => IssuePriority.Low,
                "غير مؤثرة" => IssuePriority.Unknown,

                "غير معروفة" => IssuePriority.Unknown,

                _ => IssuePriority.Unknown
            };
        }
    }
}

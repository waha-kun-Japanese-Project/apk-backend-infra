using AutoMapper;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Refit;
using Report.Client.AbstructServices;
using Report.Domain.Contracts;
using Report.Domain.Entities.Issue;
using Report.ServiceAbstraction;
using Report.Shared.DTOS.Client;
using Report.Shared.DTOS.Report;

using System.Security.Claims;


namespace Report.Service.Services;

public class IssueService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IHttpContextAccessor httpContextAccessor,
    IAiVisionClient aiVisionClient,
    IMediaStorageGrpcClient mediaStorageGrpc,
    IBackgroundJobClient backgroundJobClient) : IIssueService
{
    public async Task<AiAnalysisResponse> AnalyzeIssueAsync(
        AnalyzeIssueRequest request,
        CancellationToken cancellationToken = default)
    {
        ValidateRequest(request);

        var filePath = await UploadPhotoAsync(request.Photo!, cancellationToken);

        var prediction = await AnalyzePhotoAsync(
            request.Photo!,
            cancellationToken);

        var aiAnalysis = CreateAiAnalysis(prediction, filePath);

        await SaveAnalysisAsync(aiAnalysis, cancellationToken);

        EnqueueIssueCreation(request, prediction, aiAnalysis.IssueAttachmentId);

        return mapper.Map<AiAnalysisResponse>(prediction) with
        {
            FilePath = filePath
        };
    }

    private static void ValidateRequest(AnalyzeIssueRequest request)
    {
        if (request.Photo is null)
            throw new ArgumentNullException(
                nameof(request.Photo),
                "Photo is required for analysis.");
    }

    private async Task<string> UploadPhotoAsync(
        IFormFile photo,
        CancellationToken cancellationToken)
    {
        return await mediaStorageGrpc.UploadAsync(
            photo,
            cancellationToken);
    }

    private async Task<AiPredictionResponse> AnalyzePhotoAsync(
        IFormFile photo,
        CancellationToken cancellationToken)
    {
        await using var stream = photo.OpenReadStream();

        var prediction = await aiVisionClient.PredictAsync(
            new StreamPart(
                stream,
                photo.FileName,
                photo.ContentType));

        if (prediction.Status != "success")
        {
            throw new InvalidOperationException(
                prediction.Message ??
                "The vision service couldn't analyze the image.");
        }

        return prediction;
    }

    private AiAnalysis CreateAiAnalysis(
        AiPredictionResponse prediction,
        string filePath)
    {
        var attachment = mapper.Map<IssueAttachment>(filePath);

        var analysis = mapper.Map<AiAnalysis>(prediction);

        analysis.IssueAttachment = attachment;

        return analysis;
    }

    private async Task SaveAnalysisAsync(
        AiAnalysis analysis,
        CancellationToken cancellationToken)
    {
        unitOfWork
            .GetRepository<IssueAttachment, Guid>()
            .Add(analysis.IssueAttachment!);

        unitOfWork
            .GetRepository<AiAnalysis, Guid>()
            .Add(analysis);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private void EnqueueIssueCreation(
        AnalyzeIssueRequest request,
        AiPredictionResponse prediction,
        Guid? attachmentId)
    {
        var priority = GetPriority(prediction.Severity);

        if (priority is IssuePriority.Low or IssuePriority.Unknown)
            return;

        var createRequest = new CreateIssueRequest(
            Title: prediction.ProblemCode,
            Description: prediction.Explanation,
            ReporterId: GetLoggedInUserId(),
            Priority: priority,
            Longitude: request.Longitude,
            Latitude: request.Latitude,
            IssueAttachmentId: attachmentId!.Value);

        backgroundJobClient.Enqueue<IIssueCreationJob>(
            job => job.CreateIssueAsync(createRequest));
    }

    private Guid GetLoggedInUserId()
    {
        var user = httpContextAccessor.HttpContext?.User;

        if (user?.Identity?.IsAuthenticated != true)
            throw new UnauthorizedAccessException(
                "User not authenticated.");

        var userId = user.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userId, out var id))
            throw new UnauthorizedAccessException(
                "User Id not found in token.");

        return id;
    }

    private static IssuePriority GetPriority(string? severity)
    {
        return severity?.Trim() switch
        {
            "حرجة جداً" or "حرجة" or "عالية جداً"
                => IssuePriority.Critical,

            "عالية"
                => IssuePriority.High,

            "متوسطة" or "منخفضة"
                => IssuePriority.Medium,

            "بسيطة" or "بسيطة جداً"
                => IssuePriority.Low,

            "غير مؤثرة" or "غير معروفة"
                => IssuePriority.Unknown,

            _ => IssuePriority.Unknown
        };
    }
}

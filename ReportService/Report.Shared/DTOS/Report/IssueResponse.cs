using Report.Domain.Entities.Issue;

namespace Report.Shared.DTOS.Report
{
    public record IssueResponse(
        Guid Id,
        string Title,
        string? Description,
        string ActionRepair,
        string Type,
        string Status,
        string Priority,
        Guid ReporterId,
        Guid? AssignedExpertId,
        string? Latitude,
        string? Longitude,
        DateTime CreatedAt,
        IEnumerable<IssueAttachmentResponse> Attachments
    );
}

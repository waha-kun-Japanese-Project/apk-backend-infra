namespace Issue.Domain.Entities.Issue
{
    public class IssueAttachment : BaseEntity<Guid>
    {
        public IssueAttachmentType Type { get; set; }
        public IssueAttachmentPurpose Purpose { get; set; } = IssueAttachmentPurpose.ProblemReport;
        public string Url { get; set; } = null!;

        public Guid? IssueId { get; set; }
        public Issue? Issue { get; set; } = null!;

        public AiAnalysis? AiAnalysis { get; set; }
    }
}
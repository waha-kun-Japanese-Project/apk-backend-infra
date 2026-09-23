using System;

namespace Issue.Domain.Entities.Issue;

public class IssueShared : BaseEntity<Guid>
{
    public Guid IssueId { get; set; }

    public Issue Issue { get; set; } = null!;

    public Guid UserId { get; set; }
}
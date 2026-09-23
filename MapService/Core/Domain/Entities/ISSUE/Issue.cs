
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Map.Domain.Entities.ISSUE;
public class Issue : BaseEntity<Guid>
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Type { get; set; } = string.Empty; // e.g. "leak", "distribution_problem"
    public IssuePriority Priority { get; set; } = IssuePriority.Medium;
    public IssueStatus Status { get; set; } 
    public Guid ReporterId { get; set; } // user who reported the issue
    public Guid? AssignedExpertId { get; set; } // expert assigned to the issue

    public Guid GPSLocationId { get; set; }

    public GPSLocation GPSLocation { get; set; } = null!;
    public ICollection<IssueAttachment> IssueAttachments { get; set; } = new List<IssueAttachment>();

}

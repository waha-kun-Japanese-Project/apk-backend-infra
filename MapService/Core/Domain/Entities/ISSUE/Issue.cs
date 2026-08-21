using Map.Domain.Entities.REPORT;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Map.Domain.Entities.ISSUE;
public class Issue 
{
    public  Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public IssuePriority Priority { get; set; } = IssuePriority.Medium;

    public string Type { get; set; } = string.Empty; // e.g. "leak", "distribution_problem"
    public  Guid ReportId { get; set; }
    public Guid ReporterId { get; set; } // user who reported the issue
    public Guid? AssignedExpertId { get; set; } // expert assigned to the issue
    
}

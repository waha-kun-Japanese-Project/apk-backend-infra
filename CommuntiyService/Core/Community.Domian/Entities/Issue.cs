using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Community.Domain.Entities
{
    public class Issue :Basentity<Guid>
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Type { get; set; } = string.Empty; // e.g. "leak", "distribution_problem"
        public Guid ReporterId { get; set; } // user who reported the issue
        public Guid? AssignedExpertId { get; set; } // expert assigned to the issue
        public ICollection<IssueAttachment> Attachment { get; set; }

        public Guid ReportId { get; set; }

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<IssueVote> Votes { get; set; } = new List<IssueVote>();
        public ICollection<IssueShared> Shares { get; set; } = new List<IssueShared>();
    }
}

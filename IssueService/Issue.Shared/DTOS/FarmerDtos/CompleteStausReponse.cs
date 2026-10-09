using Issue.Domain.Entities.Issue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Shared.DTOS.FarmerDtos
{
    public class CompleteStausReponse
    {
        public Guid IssueId { get; set; }

        public DateTime ChangedAt { get; set; }

        public Guid ReporterId { get; set; }
        public IssueStatus status { get; set; }
    }
}

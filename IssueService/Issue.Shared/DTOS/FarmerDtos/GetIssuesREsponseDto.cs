using Issue.Domain.Entities.Issue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Shared.DTOS.FarmerDtos
{
    public class GetIssuesREsponseDto
    {

        public string Title { get; set; }
        public string Description { get; set; }
        public string? ImageUrl { get; set; }
        public Guid IssueId { get; set; }
        public Guid UserId { get; set; }
        public IssuePriority Priority { get; set; }
        public IssueStatus Status { get; set; }
        public string userName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UserPhoto { get; set; }
        public int CommentCount { get; set; }
        public int VoteCount { get; set; }
        public int ShareCount { get; set; }
    }
}

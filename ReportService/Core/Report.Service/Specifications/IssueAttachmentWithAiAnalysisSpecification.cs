using Report.Domain.Entities.Issue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Service.Specifications
{
    public class IssueAttachmentWithAiAnalysisSpecification
     : BaseSpecification<IssueAttachment>
    {
        public IssueAttachmentWithAiAnalysisSpecification(
            Guid issueAttachmentId)
            : base(x => x.Id == issueAttachmentId)
        {
            AddInclude(x => x.AiAnalysis);
        }
    }
}

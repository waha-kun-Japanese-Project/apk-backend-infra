using Report.Shared.DTOS.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Report.ServiceAbstraction
{
    public  interface IIssueCreationJob
    {
        Task CreateIssueAsync(CreateIssueRequest createIssueRequest);
    }
}

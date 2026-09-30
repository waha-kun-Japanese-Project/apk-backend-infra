using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Shared.DTOS.Expert
{
    public  record ResolutionActionResponse(Guid Id,string ActionRepair,string Status, string FilePath)
    {
    }
}

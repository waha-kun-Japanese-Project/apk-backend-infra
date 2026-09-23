using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Shared.DTOS.AssignExpert
{
    public  record AssignExpertRequest(
        Guid ExpertId)
    {
    }
}

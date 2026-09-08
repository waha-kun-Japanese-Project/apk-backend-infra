using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.shared.DTOS
{
    public  record ExpertDetailsResponse(Guid ExpertId, string Name)
    {
    }
}

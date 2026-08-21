using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Dashboard.Domain.Entities
{
    public enum UserStatus
    {
        Approved = 1,
        Pending =2,
        Rejected=3,
        Suspended=4
    }
}

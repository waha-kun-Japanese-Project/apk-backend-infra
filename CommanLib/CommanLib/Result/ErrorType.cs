using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommanLib.Result
{
    public enum ErrorType
    {
        Failure = 0,

        Validation = 1,

        NotFound = 2,

        Conflict = 3,

        Unauthorized = 4
    }
}

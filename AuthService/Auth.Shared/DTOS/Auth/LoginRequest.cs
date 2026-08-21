using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Shared.DTOS.Auth
{
    public record LoginRequest
    {
        public string PhoneNumber { get; set; }
    }
}

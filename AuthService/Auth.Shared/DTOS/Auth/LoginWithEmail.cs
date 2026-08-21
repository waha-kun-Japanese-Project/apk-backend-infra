using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Shared.DTOS.Auth
{
    public record LoginWithEmail
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}

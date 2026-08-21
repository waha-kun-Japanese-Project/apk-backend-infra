using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Shared.DTOS.FireBase
{
    public class LoginWithFireBaseResponseDto
    {
        public string AccessToken { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;

        public DateTime AccessTokenExpiration { get; set; }

        public DateTime RefreshTokenExpiration { get; set; }
    }
}

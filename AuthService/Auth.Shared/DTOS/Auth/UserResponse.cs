using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Shared.DTOS.Auth
{
    public record UserResponse(
        string? accesstoken,
        string? refershtoken,
        string FullName,
        string message


    ){}

}

using Auth.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Shared.DTOS.Auth
{
    public record LoginWithEmailResponse(string FulltName,
        string? refreshToken,
        string? accessToken ,
        string email)
    {}
}
using Auth.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Domain.Contracts
{
    public interface ITokenService
    { 
         string  GenerateAccessToken(AppUser appUser, IList<string> roles);
        Task<RefreshToken> CreateRefreshTokenAsync(Guid userId);

        Task<bool> ValidateRefreshTokenAsync(string token);

        Task RevokeRefreshTokenAsync(string token);

        Task<RefreshToken?> GetByRefreshTokenAsync(string token);

    }
}

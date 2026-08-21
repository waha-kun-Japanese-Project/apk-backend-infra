using Auth.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Domain.Contracts
{
    public  interface ITokenRefreshRepository
    {
        Task<RefreshToken?> GetByTokenAsync(string token);

        Task AddAsync(RefreshToken refreshToken);

        Task UpdateAsync(RefreshToken refreshToken);

        Task RevokeAsync(string token);

        Task<List<RefreshToken>> GetUserTokensAsync(Guid userId);
    }
}

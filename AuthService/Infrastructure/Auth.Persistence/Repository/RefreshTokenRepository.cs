using Auth.Domain.Contracts;
using Auth.Domain.Entities;
using Auth.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Persistence.Repository
{
    internal class RefreshTokenRepository(AppDbContext appContext) : ITokenRefreshRepository
    {
        public async Task AddAsync(RefreshToken refreshToken)
        {
            await appContext.RefreshTokens.AddAsync(refreshToken);
            await appContext.SaveChangesAsync();


        }

        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
          return  await appContext.RefreshTokens.Include(x => x.User).FirstOrDefaultAsync(x => x.Token == token);
            

        }

        public async Task<List<RefreshToken>> GetUserTokensAsync(Guid userId)
        {
          return   await appContext.RefreshTokens.Where(x => x.UserId == userId).ToListAsync();


        }

        public async Task RevokeAsync(string token)
        {
            await appContext.RefreshTokens.Where(x => x.Token == token).ForEachAsync(x => x.RevokedAt = DateTime.UtcNow);

        }

        public async Task UpdateAsync(RefreshToken refreshToken)
        {
            appContext.RefreshTokens.Update(refreshToken);
            await appContext.SaveChangesAsync();


        }
    }
}

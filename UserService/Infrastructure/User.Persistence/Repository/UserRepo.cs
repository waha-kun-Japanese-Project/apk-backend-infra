using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Contract;
using User.Domain.Entities;
using User.Persistence.Context;

namespace User.Persistence.Repository
{
    internal class UserRepo(AppDbContext appDb) : IUserRepo
    {
        public async Task DeleteAsync(Guid id)
        {

            var user = await GetByIdAsync(id);

            if (user is null)
                throw new Exception("User not found");

            appDb.Set<AppUser>().Remove(user);
            if (!await SaveChangesAsync())
                throw new Exception("Failed to delete user");
        }

        public async Task<IEnumerable<AppUser>> GetAllAsync()
        {

            return await appDb.Set<AppUser>().ToListAsync();
        }

        public async Task<AppUser?> GetByIdAsync(Guid id)
        { 
            return await appDb.Set<AppUser>().Include(u=> u.Address)
                .FirstOrDefaultAsync(x => x.Id == id);

        }

        public async Task<bool> SaveChangesAsync()
        {
            return await appDb.SaveChangesAsync() > 0;
        }

        public async Task UpdateAsync(AppUser user)
        {
            appDb.Set<AppUser>().Update(user);
            if (!await SaveChangesAsync())
                throw new Exception("Failed to update user");

        }
    }
}

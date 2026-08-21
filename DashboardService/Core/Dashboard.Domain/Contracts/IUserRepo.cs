using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dashboard.Domain.Entities;

namespace Dashboard.Domain.Contracts
{
  public interface IUserRepo
    {
       Task<IEnumerable<AppUser>> GetAllAsync();
        Task <AppUser> GetByIdAsync(Guid id);
        Task DeleteAsync(Guid id);
        Task UpdateAsync(AppUser user);
      Task <bool> SaveChangesAsync();
    }
}

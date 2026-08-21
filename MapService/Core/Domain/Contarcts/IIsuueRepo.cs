using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Map.Domain.Contarcts
{
    public interface IIsuueRepo
    {
        Task<Issue.Domain.Entities.Issue.Issue?> GetByIdAsync(Guid id);

        Task<IEnumerable<Issue.Domain.Entities.Issue.Issue>> GetAllAsync();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Domain.Entities
{
    public class UserPermission
    {
        public Guid UserId { get; set; }

        public AppUser User { get; set; } = null!;

        public int PermissionId { get; set; }

        public Permission Permission { get; set; } = null!;

        public bool IsGranted { get; set; } = true;
    }
}

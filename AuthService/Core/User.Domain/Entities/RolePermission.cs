using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Domain.Entities
{
    public class RolePermission
    {
        public Guid RoleId { get; set; }

        public AppRole Role { get; set; } = null!;

        public int PermissionId { get; set; }

        public Permission Permission { get; set; } = null!;
    }
}

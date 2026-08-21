using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Domain.Entities
{
    public class Permission
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<RolePermission> RolePermissions { get; set; }
            = new List<RolePermission>();

        public ICollection<UserPermission> UserPermissions { get; set; }
            = new List<UserPermission>();

    }
}

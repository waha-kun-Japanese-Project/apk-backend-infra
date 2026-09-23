using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Community.Domain.Entities;
namespace Community.Domain.Entities
{
    public class IssueShared : Basentity<Guid>
    {
        public Guid IssueId { get; set; }
        public Issue Issue { get; set; } = null!;
        public Guid UserId { get; set; }
    }
}

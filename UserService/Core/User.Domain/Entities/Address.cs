using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Domain.Entities
{
    public class Address
    {
        public  int Id { get; set; }
        public AppUser User { get; set; }  
        public Guid UserId { get; set; }

        public string Region { get; set; } = string.Empty;
        public string Village { get; set; } = string.Empty;

    }
}

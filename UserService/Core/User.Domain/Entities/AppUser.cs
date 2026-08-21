using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Domain.Entities
{
        public class AppUser : IdentityUser<Guid>
        {
        public string FullName { get; set; } = string.Empty;
        public Address Address { get; set; }


        public UserStatus Status { get; set; } = UserStatus.Approved;


        public string pictures { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastUpdatedAt { get; set; }
       
        public ICollection<RefreshToken> RefreshTokens { get; set; }
            = new List<RefreshToken>();


    }

}


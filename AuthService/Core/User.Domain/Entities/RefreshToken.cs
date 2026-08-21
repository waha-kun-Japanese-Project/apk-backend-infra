using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Domain.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; set; }

        public string Token { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public DateTime ExpiresAt { get; set; }

        public DateTime? RevokedAt { get; set; }

        public string? ReplacedByToken { get; set; }

        public Guid UserId { get; set; }

        public AppUser User { get; set; } = null!;

        public bool IsExpired =>
            DateTime.UtcNow >= ExpiresAt;

        public bool IsRevoked =>
            RevokedAt.HasValue;

        public bool IsActive =>
            !IsRevoked && !IsExpired;
    }
}

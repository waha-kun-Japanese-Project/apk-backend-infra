using Auth.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.shared.DTOS
{
    public record UserDetailsResponse(
        Guid Id,
        string FullName,
        string Email,
        string PhoneNumber,
        string Picture,
        string village,
        UserStatus Status,
        string Region
        )
    {
        public UserDetailsResponse() : this(Guid.Empty, "", "", "", "","" , UserStatus.Approved, "") { }
    }
}

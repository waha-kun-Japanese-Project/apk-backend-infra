using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.shared.DTOS
{
    public record UserDetailsResponse(
        Guid Id,
        string FullName,
        string Email,
        string PhoneNumber,
        string Picture,
        string village,
        string Region
        )
    {
        public UserDetailsResponse() : this(Guid.Empty, "", "", "", "", "", "") { }
    }
}

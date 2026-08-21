using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.shared.DTOS
{
    public record UserUpdateRequest(
    string? FullName,
    string? Picture,
    string? Village,
    string? Region,
    string? PhoneNumber,
    string? Email
);
}

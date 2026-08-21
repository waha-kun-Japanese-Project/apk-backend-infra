using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Domain.Contracts
{
    public  interface IOtpRepository
    {
        Task SaveOtpAsync(string phoneNumber, string code);

        Task<string?> GetOtpAsync(string phoneNumber);

        Task RemoveOtpAsync(string phoneNumber);
    }
}

using Auth.Shared.DTOS.OTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.ServiceAbstraction
{
    public  interface IOTPService
    {
       string GenerateOtpAsync();

        Task<OTPResponse> SendOtpAsync(string phoneNumber);

        Task<bool> ValidateOtpAsync(VerifyOTPRequest validateOtp);
    }
}

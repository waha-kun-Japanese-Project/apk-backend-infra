using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Shared.DTOS.OTP
{
    public  record VerifyOTPRequest(
        string phonenumber,
        string otp
        )
    {
    }
}

using Auth.Shared.DTOS.Auth;
using Auth.Shared.DTOS.FireBase;
using Auth.Shared.DTOS.OTP;
using Auth.Shared.DTOS.Token;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.ServiceAbstraction
{
    public  interface IAuthService
    {
        Task<LoginWithFireBaseResponseDto> RegisterAsync(RegisterRequest registerRequest);
        Task<OTPResponse> LoginAsync(LoginRequest loginRequest);
        Task<LoginWithEmailResponse> LoginWithEmailAsync(LoginWithEmail loginWithEmail);
        Task<LoginWithFireBaseResponseDto> FireBaseLoginAsync(FireBaseLoginDto request);
        //Task<UserResponse> VerifyOTPAsync(VerifyOTPRequest verifyOTPRequest);
        Task LogoutAsync(string refreshToken);
        Task ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
        Task ForgetPasswordasync(ForgetPassowrdDto passowrdDto);
        Task CreateAccountExpertAsync(RegisterRequest registerRequest);
        Task<LoginWithEmailResponse> RefreshTokenAsync(RefreshTokenRequest request);
    }
}

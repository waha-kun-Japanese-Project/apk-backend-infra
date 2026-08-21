using Auth.ServiceAbstraction;
using Auth.Shared.DTOS.Auth;
using Auth.Shared.DTOS.FireBase;
using Auth.Shared.DTOS.OTP;
using Auth.Shared.DTOS.Token;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Auth.presentation_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromForm]RegisterRequest registerRequest)
        {
            var result = await authService.RegisterAsync(registerRequest);
            return Ok(result);
        }
        [HttpPost("firebase-login")]
        public async Task<IActionResult> FirebaseLogin(
        FireBaseLoginDto dto)
        {
            var result =
                await authService.FireBaseLoginAsync(dto);

            return Ok(result);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var result = await authService.LoginAsync(loginRequest);
            return Ok(result);

        }
        [HttpPost("LoginWithEmail")]
        public async Task<IActionResult> LoginWithEmail([FromBody] LoginWithEmail loginWithEmail)
        {
            var result = await authService.LoginWithEmailAsync(loginWithEmail);
            return Ok(result);
        }

        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPassowrdDto forgetPasswordDto)
        {
            await authService.ForgetPasswordasync(forgetPasswordDto);
            return Ok();
        }
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            await authService.ResetPasswordAsync(resetPasswordDto);
            return Ok();
        }
        //[HttpPost("Verify-OTP")]
        //public async Task<IActionResult> VerifyOTP([FromBody] VerifyOTPRequest verifyOTPRequest)
        //{
        //    var result = await authService.VerifyOTPAsync(verifyOTPRequest);
        //    return Ok(result);
        //}
        [HttpPost("CreateExpert")]
        public async Task<IActionResult> CreateExpert([FromForm]  RegisterRequest registerRequest)
        {
                await authService.CreateAccountExpertAsync(registerRequest);
                return Ok();


        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout([FromBody] string refreshToken)
        {
            await authService.LogoutAsync(refreshToken);
            return Ok();
        }
        [HttpPost("Refresh-Token")]
        public async Task<IActionResult> RefreshToken( RefreshTokenRequest request)
        {
            var result = await authService.RefreshTokenAsync(request);
        

            return Ok(result);
        }


    }
}

using Auth.Domain.Contracts;
using Auth.Domain.Entities;
using Auth.ServiceAbstraction;
using Auth.Shared.DTOS.Auth;
using Auth.Shared.DTOS.OTP;
using Auth.Shared.DTOS.Token;
using CommanLib.EventNotification.EmailEvent;
using MassTransit;
using MassTransit.Transports;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Notification.Service;
using FirebaseAdmin.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.Http;
using Twilio.TwiML.Messaging;
using Twilio.Types;
using static System.Net.WebRequestMethods;
using Notification.ServicesAbstract;
using Auth.Shared.DTOS.FireBase;

namespace Auth.Service
{
    public class AuthService(
        IFireBaseService FireBaseService,
    UserManager<AppUser> userManager
    , IOTPService otpService
     , ITokenService tokenService
        , IPublishEndpoint publish
        , IWebHostEnvironment webHostEnvironment) : IAuthService
    {
      
        private static readonly Dictionary<string, string> AllowedPictureTypes = new(StringComparer.OrdinalIgnoreCase)
        {
            ["image/jpeg"] = ".jpg",
            ["image/jpg"] = ".jpg",
            ["image/png"] = ".png",
            ["image/webp"] = ".webp",
            ["image/svg+xml"] = ".svg",
        };

        private async Task<string> SavePictureAsync(IFormFile? picture)
        {
            if (picture == null || picture.Length == 0)
            {
                return string.Empty;
            }

            if (!AllowedPictureTypes.TryGetValue(picture.ContentType, out var extension))
            {
                throw new Exception(
                    $"Unsupported picture type '{picture.ContentType}'. Allowed types: jpg, png, webp, svg.");
            }

            var uploadsRoot = Path.Combine(webHostEnvironment.WebRootPath ?? "wwwroot", "uploads");
            Directory.CreateDirectory(uploadsRoot);

            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadsRoot, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await picture.CopyToAsync(stream);
            }

            return $"/uploads/{fileName}";
        }

        public async Task ForgetPasswordasync(ForgetPassowrdDto passowrdDto)
        {
            var user = await userManager.FindByEmailAsync(passowrdDto.Email!);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var param = new Dictionary<string, string?>
            {
                {"token", token },
                { "email", passowrdDto.Email! }
            };
            var callBack = QueryHelpers.AddQueryString(passowrdDto.ClinetUrl!, param);
            var message = new Message(user.Email, "Reset Password", callBack, null);
            await publish.Publish(new ResetPasswordEvent(user.Email, callBack));
        }

        public async Task ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            var user = await userManager.FindByEmailAsync(resetPasswordDto.Email!);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            var result = await userManager.ResetPasswordAsync(user, resetPasswordDto.token!, resetPasswordDto.Password!);
            if (!result.Succeeded)
            {
                throw new Exception("Password reset failed");
            }

        }
        public async Task<OTPResponse> LoginAsync(LoginRequest loginRequest)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == loginRequest.PhoneNumber);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            var result = await otpService.SendOtpAsync(loginRequest.PhoneNumber);
            return result;

        }

        public async Task<LoginWithEmailResponse> LoginWithEmailAsync(LoginWithEmail loginWithEmail)
        {

            var user = await userManager.FindByEmailAsync(loginWithEmail.Email);

            if (user == null)
                throw new Exception("invalid email or password");

            var isPasswordValid = await userManager.CheckPasswordAsync(user, loginWithEmail.Password);

            if (!isPasswordValid)
                throw new Exception("invalid email or password");

            if (!(user.Status == UserStatus.Approved))
                throw new Exception("User is not approved");

            var refreshtoken = await tokenService.CreateRefreshTokenAsync(user.Id);

            var accessToken = tokenService.GenerateAccessToken(user, await userManager.GetRolesAsync(user));

            return new LoginWithEmailResponse(
                FulltName: user.FullName, refreshToken: refreshtoken.Token, accessToken: accessToken, email: user.Email
            );
        }

        public async Task LogoutAsync(string refreshToken)
        {
            await tokenService.RevokeRefreshTokenAsync(refreshToken);

        }

        public async Task<LoginWithFireBaseResponseDto> RegisterAsync(RegisterRequest registerRequest)
        {
            //var existingUser = await userManager.Users.FirstOrDefaultAsync(
            //    u => u.PhoneNumber == registerRequest.PhoneNumber ||
            //         u.Email == registerRequest.email);

            //if (existingUser != null)
            //{
            //    if (existingUser.PhoneNumber == registerRequest.PhoneNumber)
            //        throw new Exception("Phone number is already registered.");

            //    if (existingUser.Email == registerRequest.email)
            //        throw new Exception("Email is already registered.");
            //}

            //var pictureUrl = await SavePictureAsync(registerRequest.picture);

            //var user = new AppUser
            //{
            //    FullName = registerRequest.FullName,
            //    UserName = registerRequest.email, // Changed from FullName
            //    PhoneNumber = registerRequest.PhoneNumber,
            //    Email = registerRequest.email,
            //    Address = new Address
            //    {
            //        Village = registerRequest.village,
            //        Region = registerRequest.Region
            //    },
            //    pictures = pictureUrl
            //};

            //var result = await userManager.CreateAsync(user, registerRequest.password);

            //if (!result.Succeeded)
            //{
            //    var errors = string.Join(" | ",
            //        result.Errors.Select(e => $"{e.Code}: {e.Description}"));

            //    throw new Exception($"User creation failed: {errors}");
            //}

            //var roleResult = await userManager.AddToRoleAsync(user, "Farmer");

            //if (!roleResult.Succeeded)
            //{
            //    var errors = string.Join(" | ",
            //        roleResult.Errors.Select(e => $"{e.Code}: {e.Description}"));

            //    throw new Exception($"Failed to add role: {errors}");
            //}

            //var otp = await otpService.SendOtpAsync(registerRequest.PhoneNumber);


            //return otp;
            var user = CreateUserFromFirebaseAsync(registerRequest);
            return await BuildLoginResponseAsync(await user);
        }

    //    public async Task<UserResponse> VerifyOTPAsync(VerifyOTPRequest verifyOTPRequest)
    //    {
    //        var result = await otpService.ValidateOtpAsync(verifyOTPRequest);
    //        if (!result)
    //        {
    //            throw new Exception("Invalid OTP");
    //        }

    //        var user = userManager.Users.FirstOrDefault(x => x.PhoneNumber == verifyOTPRequest.phonenumber);
    //        if (user == null)
    //        {
    //            throw new Exception("User not found");
    //        }

    //        var roles = await userManager.GetRolesAsync(user);
    //        if (!roles.Any())
    //        {
    //            await userManager.AddToRoleAsync(user, "Farmer");
    //        }

    //        var refreshtoken = await tokenService.CreateRefreshTokenAsync(user.Id);


    //        var accesstoken = tokenService.GenerateAccessToken(user, roles);

    //        var userResponse = new UserResponse(
    //        accesstoken: accesstoken,
    //        refershtoken: refreshtoken.Token,
    //        FullName: user.FullName,
    //        message: "OTP verified successfully"
    //);

    //        return userResponse;
    //    }

        public async Task CreateAccountExpertAsync(RegisterRequest registerRequest)
        {
            var existingUser = await userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == registerRequest.PhoneNumber || u.Email == registerRequest.email);

            if (existingUser != null)
            {
                if (existingUser.PhoneNumber == registerRequest.PhoneNumber)
                    throw new Exception("Phone number is already registered.");

                if (existingUser.Email == registerRequest.email)
                    throw new Exception("Email is already registered.");
            }

            var pictureUrl = await SavePictureAsync(registerRequest.picture);

            var user = new AppUser
            {
                FullName = registerRequest.FullName,
                UserName = registerRequest.PhoneNumber,
                Status = UserStatus.Pending,
                PhoneNumber = registerRequest.PhoneNumber,
                Email = registerRequest.email,
                Address = new Address
                {
                    Village = registerRequest.village,
                    Region = registerRequest.Region
                },
                pictures = pictureUrl
            };
            var result = await userManager.CreateAsync(user, registerRequest.password);

            if (!result.Succeeded)
            {
                var errors = string.Join(" | ", result.Errors.Select(e => $"{e.Code}: {e.Description}"));
                throw new Exception($"User creation failed: {errors}");
            }
            await userManager.AddToRoleAsync(user, "Expert");
            await publish.Publish(new AccountEvent(user.Email, user.FullName));
        }


        public async Task<LoginWithEmailResponse> RefreshTokenAsync(RefreshTokenRequest request)
        {
            var token = await tokenService.GetByRefreshTokenAsync(request.RefreshToken);
            if (token == null || token.RevokedAt != null || token.ExpiresAt < DateTime.UtcNow)
            {
                throw new Exception("Invalid refresh token");
            }

            var refreshtoken = tokenService.CreateRefreshTokenAsync(token.UserId);
            var accesstoken = tokenService.GenerateAccessToken(await userManager.FindByIdAsync(token.UserId.ToString()), await userManager.GetRolesAsync(await userManager.FindByIdAsync(token.UserId.ToString())));
            await tokenService.RevokeRefreshTokenAsync(request.RefreshToken);
            return new LoginWithEmailResponse(
                FulltName: (await userManager.FindByIdAsync(token.UserId.ToString())).FullName,
                refreshToken: (await refreshtoken).Token,
                accessToken: accesstoken,
                email: (await userManager.FindByIdAsync(token.UserId.ToString())).Email
                );
        }

        public async Task<LoginWithFireBaseResponseDto> FireBaseLoginAsync(FireBaseLoginDto request)
        {
            var user =  GetUserFromFirebaseAsync(request.IdToken);
            return await BuildLoginResponseAsync(await user);
        }
        private async Task<AppUser> GetUserFromFirebaseAsync(string idToken)
        {
            var firebaseUser = await FireBaseService.VerifyTokenAsync(idToken);

            var user = await userManager.Users
                .FirstOrDefaultAsync(x => x.PhoneNumber == firebaseUser.PhoneNumber);

            if (user == null)
            {
                user = new AppUser
                {
                    UserName = firebaseUser.PhoneNumber,
                    PhoneNumber = firebaseUser.PhoneNumber,
                    PhoneNumberConfirmed = true
                };

                var result = await userManager.CreateAsync(user);
                if (!result.Succeeded)
                    throw new Exception(
                        "Cannot create user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            return user;
        }
        private async Task<AppUser> CreateUserFromFirebaseAsync(RegisterRequest registerRequest)
        {
            var firebaseUser = await FireBaseService.VerifyTokenAsync(registerRequest.IdToken);

            // Trust the phone number from the verified Firebase token, not the client-submitted one
            var phoneNumber = firebaseUser.PhoneNumber;

            var existingUser = await userManager.Users.FirstOrDefaultAsync(
                u => u.PhoneNumber == phoneNumber || u.Email == registerRequest.email);

            if (existingUser != null)
            {
                if (existingUser.PhoneNumber == phoneNumber)
                    throw new Exception("Phone number is already registered.");
                if (existingUser.Email == registerRequest.email)
                    throw new Exception("Email is already registered.");
            }

            var pictureUrl = await SavePictureAsync(registerRequest.picture);

            var user = new AppUser
            {
                FullName = registerRequest.FullName,
                UserName = registerRequest.email,
                PhoneNumber = phoneNumber,
                PhoneNumberConfirmed = true, // verified via Firebase
                Email = registerRequest.email,
                Address = new Address
                {
                    Village = registerRequest.village,
                    Region = registerRequest.Region
                },
                pictures = pictureUrl
            };

            var result = await userManager.CreateAsync(user, registerRequest.password);
            if (!result.Succeeded)
            {
                var errors = string.Join(" | ", result.Errors.Select(e => $"{e.Code}: {e.Description}"));
                throw new Exception($"User creation failed: {errors}");
            }

            var roleResult = await userManager.AddToRoleAsync(user, "Farmer");
            if (!roleResult.Succeeded)
            {
                var errors = string.Join(" | ", roleResult.Errors.Select(e => $"{e.Code}: {e.Description}"));
                throw new Exception($"Failed to add role: {errors}");
            }

            return user;
        }

        private async Task<LoginWithFireBaseResponseDto> BuildLoginResponseAsync(AppUser user)
        {
            var jwt = await tokenService.CreateRefreshTokenAsync(user.Id);
            var accessToken = tokenService.GenerateAccessToken(user, await userManager.GetRolesAsync(user));

            return new LoginWithFireBaseResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = jwt.Token,
                AccessTokenExpiration = DateTime.UtcNow.AddDays(2), // see note below
                RefreshTokenExpiration = jwt.ExpiresAt.AddDays(2)
            };
        }
    }
}

using Auth.Domain.Contracts;
using Auth.ServiceAbstraction;
using Auth.Shared.DTOS.OTP;
using CommanLib.EventNotification.SmsEvent;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Service
{
    public class OtpService(IOtpRepository otpRepository, IPublishEndpoint  publish) : IOTPService
    {
        public string GenerateOtpAsync()
        {

            var otp = Random.Shared
            .Next(100000, 999999)
            .ToString();


            return otp;


        }

        public async Task<OTPResponse> SendOtpAsync(string phoneNumber)
        {
            var otp = GenerateOtpAsync();
            var check = await otpRepository.GetOtpAsync(phoneNumber);
            if (!(check == null))
                await otpRepository.RemoveOtpAsync(phoneNumber);
            
            await otpRepository.SaveOtpAsync(phoneNumber, otp);
            //await smsService.SendAsync(phoneNumber, $"Your OTP code is: {otp}");
            await publish.Publish(new SendOtpEvent(phoneNumber, otp));
            return new OTPResponse(
               success: true,
               message: "OTP sent successfully"
            );

        }

        public async Task<bool> ValidateOtpAsync(VerifyOTPRequest validateOtp )
        {
            var storedOtp =
            await otpRepository.GetOtpAsync(validateOtp.phonenumber);

            Console.WriteLine($"Stored OTP = {storedOtp}");
            Console.WriteLine($"Request OTP = {validateOtp.otp}");
            if (storedOtp != validateOtp.otp)
                return false;
               
            await otpRepository.RemoveOtpAsync( validateOtp.phonenumber);
            return true;

           
        }
    }
}

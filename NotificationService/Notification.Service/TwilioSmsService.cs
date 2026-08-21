using Microsoft.Extensions.Configuration;
using Notification.ServicesAbstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Notification.Service
{
    public class TwilioSmsService(IConfiguration configuration) : ISmsService
    {
        public async Task SendAsync(string phoneNumber, string message)
        {
            TwilioClient.Init(
            configuration["Twilio:AccountSid"],
            configuration["Twilio:AuthToken"]);

            await MessageResource.CreateAsync(
                body: message,
                from: new Twilio.Types.PhoneNumber(
                    configuration["Twilio:FromNumber"]),
                to: new Twilio.Types.PhoneNumber(phoneNumber));


        }
    }
}

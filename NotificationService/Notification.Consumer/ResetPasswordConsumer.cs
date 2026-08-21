using CommanLib.EventNotification.EmailEvent;
using MassTransit;
using Notification.ServicesAbstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Consumer
{
    public  class ResetPasswordConsumer(IEmailService emailService) : IConsumer<ResetPasswordEvent>
    {
        public async Task Consume(ConsumeContext<ResetPasswordEvent> context)
        {

            var message = context.Message;


            await emailService.SendEmailAsync(
                message.Email,
                "Reset Password",
                message.ResetPasswordLink
            );
        }
    }
}

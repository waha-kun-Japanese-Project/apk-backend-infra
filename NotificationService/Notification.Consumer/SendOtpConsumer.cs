using CommanLib.EventNotification.SmsEvent;
using MassTransit;
using Notification.ServicesAbstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Consumer
{
    public  class SendOtpConsumer(ISmsService smsService ) : IConsumer<SendOtpEvent>
    {
        public async Task Consume( ConsumeContext<SendOtpEvent> context)
        {


            var message = context.Message;


            await smsService.SendAsync(
                message.PhoneNumber,
                message.OtpCode
            );


        }
    }
}

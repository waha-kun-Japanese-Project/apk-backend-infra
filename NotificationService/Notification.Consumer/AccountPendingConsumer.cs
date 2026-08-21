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
    public class AccountPendingConsumer(IEmailService emailService) : IConsumer<AccountEvent>
    {
        public async Task Consume(ConsumeContext<AccountEvent> context)
        {
            var message = context.Message;

            var body = $"<p>مرحبًا {message.FullName},</p>" +
                       "<p>تم استلام طلب تسجيلك كخبير بنجاح، وحسابك حاليًا قيد المراجعة من الإدارة.</p>" +
                       "<p>سيتم إعلامك عبر البريد الإلكتروني فور الموافقة على حسابك.</p>";

            await emailService.SendEmailAsync(
                message.EmailAddress,
                "حسابك قيد المراجعة",
                body
            );
        }
    }
}

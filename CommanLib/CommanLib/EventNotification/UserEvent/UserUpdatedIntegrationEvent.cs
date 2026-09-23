using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommanLib.EventNotification.UserEvent
{
    public sealed record UserUpdatedIntegrationEvent(
     Guid UserId,
     string FullName);
}

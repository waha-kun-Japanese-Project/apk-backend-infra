using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.ServicesAbstract
{
    public interface IFireBaseService
    {
        Task<FirebaseUserInfo> VerifyTokenAsync(string idToken);

    }
    public class FirebaseUserInfo
    {
        public string Uid { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;
    }
}

using FirebaseAdmin.Auth;
using Notification.ServicesAbstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Service
{
    public class FireBaseService : IFireBaseService
    {
        public async Task<FirebaseUserInfo> VerifyTokenAsync(string idToken)
        {
            if (string.IsNullOrEmpty(idToken))
            {
                throw new ArgumentException("ID token cannot be null or empty.", nameof(idToken));
            }
            FirebaseToken decodedToken;
            try
            {
                decodedToken = await FirebaseAuth.DefaultInstance
                                                 .VerifyIdTokenAsync(idToken);
            }
            catch (Exception ex)
            {
                throw new UnauthorizedAccessException("Invalid Firebase Token.", ex);
            }

            var userInfo = new FirebaseUserInfo
            {
                Uid = decodedToken.Uid,
                PhoneNumber = decodedToken.Claims.ContainsKey("phone_number") ? decodedToken.Claims["phone_number"].ToString() : string.Empty,
            };
            return userInfo;
            //await Task.CompletedTask;

            //return new FirebaseUserInfo
            //{
            //    Uid = Guid.NewGuid().ToString(),
            //    PhoneNumber = "+201012345678"
            //};
        }
    }
}

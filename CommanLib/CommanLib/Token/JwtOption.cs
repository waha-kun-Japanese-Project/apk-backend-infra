using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommanLib
{
    public  class JwtOption
    {
        public static string  SectionName = "Jwt";
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; } 
        public string Expiry { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace TokenAuthentication.Core.Entitys
{
   public class UserRefreshToken
    {
        public string UserId { get; set; }
        public string Code { get; set; }
        public DateTime Expiration { get; set; }
    }
}

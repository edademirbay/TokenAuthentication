using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace TokenAuthentication.Core.Entitys
{
    public class UserApp :IdentityUser
    {
        public string City { get; set; }
    }
}

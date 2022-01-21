using System;
using System.Collections.Generic;
using System.Text;

namespace TokenAuthentication.Core.Dtos
{
    public class ClientLoginDto
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}

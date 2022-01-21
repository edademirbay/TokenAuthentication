using System;
using System.Collections.Generic;
using System.Text;

namespace TokenAuthentication.Core.Dtos
{
    public class CreateUserDto
    {
        /// <summary>
        /// Kullanıcıdan userName almak zorunda değiliz.
        /// Genelde yaklaşım şu test@gmail.com email adresinden @ e kadar olan kısım alınır.
        /// Sonuna random ifadeler eklenir. Örn : test12345
        /// </summary>
        public string  UserName { get; set; }
        public string Email { get; set; }
        public string  Password { get; set; }
    }
}

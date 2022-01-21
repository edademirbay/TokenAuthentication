using System;
using System.Collections.Generic;
using System.Text;
using TokenAuthentication.Core.Configuration;
using TokenAuthentication.Core.Dtos;
using TokenAuthentication.Core.Entitys;

namespace TokenAuthentication.Core.Service
{
    public interface ITokenService
    {
        TokenDto CreateToken(UserApp userApp);
        ClientTokenDto CreateTokenByClient(Client client);
    }
}

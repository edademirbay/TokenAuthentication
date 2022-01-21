﻿using SharedExpression.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TokenAuthentication.Core.Dtos;

namespace TokenAuthentication.Core
{
    public interface IAuthenticationService
    {
        Task<Response<TokenDto>> CreateTokenAsync(LoginDto loginDto);
        Task<Response<TokenDto>> CreateTokenByRefreshToken(string refreshToken);
        Task<Response<NoDataDto>> RevokeRefreshToken(string refreshToken);
        Response<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto);
    }
}

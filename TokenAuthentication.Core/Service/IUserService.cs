using SharedExpression.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TokenAuthentication.Core.Dtos;

namespace TokenAuthentication.Core.Service
{
    public interface IUserService
    {
        Task<Response<UserAppDto>> CreateUserAsync(CreateUserDto createUserDto);
        Task<Response<UserAppDto>> GetUserByNameAsync(string userName);
    }
}

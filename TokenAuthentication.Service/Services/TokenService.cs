using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SharedExpression;
using SharedExpression.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TokenAuthentication.Core.Configuration;
using TokenAuthentication.Core.Dtos;
using TokenAuthentication.Core.Entitys;
using TokenAuthentication.Core.Service;

namespace TokenAuthentication.Service.Services
{
    public class TokenService : ITokenService
    {

        private readonly UserManager<UserApp> _userManager;

        private readonly CustomTokenOption _customTokenOption;

        public TokenService(UserManager<UserApp> userManager, IOptions<CustomTokenOption> options)
        {
            _userManager = userManager;
            _customTokenOption = options.Value;

        }

        private string CreateRefreshToken()
        {
            //Guid ile de yapabilirdi => new Guid
            //Kripto sınıfından random üreteç daha sağlıklıdır
            var numberByte = new Byte[32];
            var randomValue = RandomNumberGenerator.Create();
            randomValue.GetBytes(numberByte);
            return Convert.ToBase64String(numberByte);
        }

        private IEnumerable<Claim> GetClaims(UserApp userApp, List<string> audiences)
        {
            var userList = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userApp.Id),
                new Claim(JwtRegisteredClaimNames.Email, userApp.Email),
                new Claim(ClaimTypes.Name, userApp.UserName),
                //tokena random bi ıd ekliyoruz
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };


            userList.AddRange(audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));
            return userList;
        }

        private IEnumerable<Claim> GetClaimsByClient(Client client)
        {
            var claims = new List<Claim>();
            claims.AddRange(client.Audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));

            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString());
            new Claim(JwtRegisteredClaimNames.Sub, client.Id.ToString());

            return claims;
        }
        public TokenDto CreateToken(UserApp userApp)
        {
            var accessTokenExpiration = DateTime.Now.AddMinutes(_customTokenOption.AccessTokenExpiration);

            var refreshTokenExpiration = DateTime.Now.AddMinutes(_customTokenOption.RefreshTokenExpiration);


            var securityKey = SignService.GetSymmetricSecurityKey(_customTokenOption.SecurityKey);

            //tokendaki key değerini secure imzalama algoritmalardan biri ile imzalıyoruz
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            //expires ile notbefore arasındaki süreç arasında tokenım yaşar
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(issuer: _customTokenOption.Issuer,
                expires: accessTokenExpiration,
                notBefore: DateTime.Now,
                claims: GetClaims(userApp, _customTokenOption.Audience),
                signingCredentials: signingCredentials);

            //tokenı oluşturan asıl kısım burasıdır
            var handler = new JwtSecurityTokenHandler();
            var token = handler.WriteToken(jwtSecurityToken);

            var tokenDto = new TokenDto
            {
                AccessToken = token,
                RefreshToken = CreateRefreshToken(),
                AccessTokenExpiration = accessTokenExpiration,
                RefreshTokenExpiration = refreshTokenExpiration
            };

            return tokenDto;
        }

        public ClientTokenDto CreateTokenByClient(Client client)
        {
            var accessTokenExpiration = DateTime.Now.AddMinutes(_customTokenOption.AccessTokenExpiration);
            
            var securityKey = SignService.GetSymmetricSecurityKey(_customTokenOption.SecurityKey);
         
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(issuer: _customTokenOption.Issuer,
                expires: accessTokenExpiration,
                notBefore: DateTime.Now,
                claims:GetClaimsByClient(client),
                signingCredentials: signingCredentials);

           
            var handler = new JwtSecurityTokenHandler();
            var token = handler.WriteToken(jwtSecurityToken);

            var tokenDto = new ClientTokenDto
            {
                AccessToken = token,                
                AccessTokenExpiration = accessTokenExpiration,                
            };

            return tokenDto;
        }
    }
}

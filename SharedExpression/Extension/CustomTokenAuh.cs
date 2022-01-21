using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SharedExpression.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedExpression.Extension
{
   public static class CustomTokenAuh
    {      
        public static void AddCustomTokenAuth(this IServiceCollection services, CustomTokenOption tokenOptions)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = tokenOptions.Issuer,
                    ValidAudience = tokenOptions.Audience[0],
                    IssuerSigningKey = SignService.GetSymmetricSecurityKey(tokenOptions.SecurityKey),

                    ValidateIssuerSigningKey = true, //imzayı doğrula
                    ValidateAudience = true, //audienceyi doğrula
                    ValidateLifetime = true, //token canlı mı doğrula
                    // token life 1 saat ise otomatikmen 5 dk ekler yani 65 dklık token ömrü oluyor
                    //Bunu ekarte etmek için TimeSpan.zero kullanırız
                    ClockSkew = TimeSpan.Zero
                };
            });

        }
    }
}

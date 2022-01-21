using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SharedExpression;
using SharedExpression.Extension;
using SharedExpression.Services;
using TokenAuthentication.Core;
using TokenAuthentication.Core.Configuration;
using TokenAuthentication.Core.Entitys;
using TokenAuthentication.Core.Repositories;
using TokenAuthentication.Core.Service;
using TokenAuthentication.Core.UnitOfWork;
using TokenAuthentication.Data;
using TokenAuthentication.Data.Repositories;
using TokenAuthentication.Service.Services;

namespace TokenAuthentication.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //DI Register
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));           
            services.AddScoped(typeof(IGenericService<,>), typeof(GenericService<,>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("SqlServer"), sqlOptions =>
                 {
                     //Migration işlemleri nerede gerçekleşecek
                     sqlOptions.MigrationsAssembly("TokenAuthentication.Data");
                 });
            });
            services.AddIdentity<UserApp, IdentityRole>(Opt =>
            {
                Opt.User.RequireUniqueEmail = true;            
                Opt.Password.RequireNonAlphanumeric = false;

            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders(); //şifre sıfırlamalardaki default token üreteci

            //option pattern
            services.Configure<CustomTokenOption>(Configuration.GetSection("TokenOptions"));           
            services.Configure<List<Client>>(Configuration.GetSection("Clients"));

            //TOKEN DOĞRULAMA AYARLARI
            //login sistemi 1 tane o yüzden bir şemayı kullanıyor
            //auth şeması ile jwt şeması birbiri ile default challange şema üzerinden haberleşiyo
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                var tokenOptions = Configuration.GetSection("TokenOptions").Get<CustomTokenOption>();
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = tokenOptions.Issuer,
                    ValidAudience = tokenOptions.Audience[0],
                    IssuerSigningKey = SignService.GetSymmetricSecurityKey(tokenOptions.SecurityKey),

                    ValidateIssuerSigningKey = true, //imzayı doğrula
                    ValidateAudience = true, //audienceyi doğrula
                    ValidateLifetime = true, //token canlı mı doğrula
                    // token life 1 saat ise otomatikmen 5 dk ekler yani 65 dklık token ömrü oluyor
                    //Bunu ekarte etmek için TimeSpan.zero diyorum
                    ClockSkew = TimeSpan.Zero
                };
            });




            services.AddControllers();

            //Startup hangi assemblyde tokenAuthentication.API assemblysinde bu assemblydeki tüm abstractvalidatordan miras alan classları buluyor
            services.AddFluentValidation(options =>
            {
                options.RegisterValidatorsFromAssemblyContaining<Startup>();
            });
            services.UseCustomValidationResponse();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            else
            {
                app.UseCustomException();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

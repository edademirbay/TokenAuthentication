using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TokenAuthentication.Core.Entitys;

namespace TokenAuthentication.Data
{
   public class AppDbContext: IdentityDbContext<UserApp,IdentityRole,string>
    {
        public AppDbContext(DbContextOptions<AppDbContext>options):base(options)

        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }
       
        protected override void OnModelCreating(ModelBuilder builder)
        {
            //bu katmanın yani data katmanın içindeki tüm assemblynin yani dllerin implement ettiği interfaceleri alıp
            //tüm ayarları ayağa kaldıracak
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}

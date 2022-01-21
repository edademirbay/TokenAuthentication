using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace TokenAuthentication.Service
{
   public static class ObjectMapper
    {
        //ObjectMapper sınıfının Mapper metodu çağrılmadan bu kod parçası çalışmaz
        //Böylelikle bellekte datalar yer tutmaz. Lazy loading mantığı
        private static readonly Lazy<IMapper> lazy = new Lazy<IMapper>(() =>
          {
              var config = new MapperConfiguration(cfg =>
               {
                   cfg.AddProfile<DtoMapper>();
               });
              return config.CreateMapper();
          });
     
        public static IMapper Mapper => lazy.Value;
    }
}

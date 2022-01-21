using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TokenAuthentication.Core.Dtos;
using TokenAuthentication.Core.Entitys;

namespace TokenAuthentication.Service
{
    //default internal yani bu assemblyde erişilecek
    class DtoMapper:Profile
    {
        public DtoMapper()
        {

            CreateMap<ProductDto, Product>().ReverseMap();
            CreateMap<UserAppDto, UserApp>().ReverseMap();
        }
    }
}

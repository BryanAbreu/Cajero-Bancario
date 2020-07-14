using AutoMapper;
using Database.Models;
using ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Ibanking.Infraestructure.AutoMapper
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            ConfigureUser();
            ConfigureProducto();
        }
        private void ConfigureUser()
        {
            CreateMap<RegisterViewModel, Usuario>().ReverseMap()
                .ForMember(dest => dest.ConfirmPassword, opt => opt.Ignore())
                .ForMember(dest => dest.Estado, opt => opt.Ignore())
                .ForMember(dest => dest.Roles, opt => opt.Ignore())
                .ForMember(dest => dest.SelectedRol, opt => opt.Ignore());
        }
        private void ConfigureProducto()
        {
            CreateMap<ProductoViewModel, Productos>().ReverseMap();
        }

    }
}

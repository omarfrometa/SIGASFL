using AutoMapper;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using SIGASFL.Entities;
using SIGASFL.Models.Contracts.Response;
using SIGASFL.Models.Views;

namespace SIGASFL.Services.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Users, LoginResponse>().ReverseMap();

            CreateMap<Users, ProfileResponse>()
               .ForPath(profile => profile.FirstName, e => e.MapFrom(user => user.UserProfile.FirstOrDefault().FirstName))
               .ForPath(profile => profile.LastName1, e => e.MapFrom(user => user.UserProfile.FirstOrDefault().LastName1))
               .ForPath(profile => profile.LastName2, e => e.MapFrom(user => user.UserProfile.FirstOrDefault().LastName2))
               .ForPath(profile => profile.NickName, e => e.MapFrom(user => user.UserProfile.FirstOrDefault().NickName))
               .ForPath(profile => profile.Gender, e => e.MapFrom(user => user.UserProfile.FirstOrDefault().Gender))
               .ReverseMap();

            CreateMap<Roles, RolesView>()
               .ForPath(m => m.Nombre, e => e.MapFrom(i => i.Name))
               .ForPath(m => m.Descripcion, e => e.MapFrom(i => i.Description))
               .ForPath(m => m.Activo, e => e.MapFrom(i => i.Enabled))
               .ForPath(m => m.FechaCreacion, e => e.MapFrom(i => i.CreatedDate))
               .ReverseMap();
        }
    }
}

using AutoMapper;
using DoDo.Data.Entities;
using DoDo.DTOs;

namespace DoDo.MapperP
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterDto, Usuario>();
            CreateMap<Usuario, RegisterDto>();

            CreateMap<Usuario, LoginDto>(); 
        }
    }
}

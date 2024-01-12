using AutoMapper;
using AuthentifyCore.DTOs;
using AuthentifyCore.Models;

namespace AuthentifyCore.Utils
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<CreateUserDTO, User>();
            CreateMap<PatchUserDTO, User>().ReverseMap();
            CreateMap<RegisterDTO, User>();
            CreateMap<LoginDTO, User>();
        }
    }
}

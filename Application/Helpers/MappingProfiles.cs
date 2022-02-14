using Application.DTOs;
using AutoMapper;
using Domain;

namespace Application.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<AppUser, LoginDto>();
            CreateMap<AppUser, UserDto>()
                .ForMember(e => e.ApplicationRole, o =>
                    o.MapFrom(s => s.ApplicationRole.Description));
            CreateMap<ApplicationRole, ApplicationRoleDto>();
        }
    }
}
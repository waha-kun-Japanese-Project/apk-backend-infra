using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Entities;
using User.shared.DTOS;

namespace User.Services.Mapping
{
    public  class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<AppUser, UserDetailsResponse>()
               .ForMember(dest => dest.village,
                   opt => opt.MapFrom(src => src.Address.Village))
               .ForMember(dest => dest.Region,
                   opt => opt.MapFrom(src => src.Address.Region));
            


            // UserUpdateRequest -> Address
            CreateMap<UserUpdateRequest, Address>()
                .ForMember(dest => dest.Village,
                    opt => opt.MapFrom(src => src.Village))
                .ForMember(dest => dest.Region,
                    opt => opt.MapFrom(src => src.Region))
                .ForMember(dest => dest.Id,
                    opt => opt.Ignore()); // Ignore Id to prevent overwriting
            


            // UserUpdateRequest -> AppUser
            CreateMap<UserUpdateRequest, AppUser>()
                .ForMember(dest => dest.Address,
                    opt => opt.MapFrom(src => src))
                .ForAllMembers(opt =>
                    opt.Condition((src, dest, srcMember) => srcMember != null));


           






        }
    }
                  
}

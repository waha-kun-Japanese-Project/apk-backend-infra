using AutoMapper;
using Community.Domain.Entities;
using Communtiy.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Community.Service.Maaping
{
    public class Communitymap  : Profile
    {
        public Communitymap() 
        {
            CreateMap<Domain.Entities.Comment, Communtiy.Shared.CommentResponseDto>()
                .ForMember(dest => dest.Id,
                opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.IssueId,
                opt => opt.MapFrom(src => src.IssueId))
            .ForMember(dest => dest.UserId,
                opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Text,
                opt => opt.MapFrom(src => src.Text))
            .ForMember(dest => dest.CreatedAt,
                opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember
                (dest => dest.VoiceUrl,
                opt => opt.MapFrom(src => src.VoiceUrl));

        }
    }
}

using AutoMapper;
using Issue.Shared.DTOS.AssignExpert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Service.MapperingProfiles
{
    internal class IssueProfile:Profile
    {
        public IssueProfile()
        {
            CreateMap<Domain.Entities.Issue.Issue, Shared.DTOS.FarmerDtos.GetIssuesREsponseDto>()
                .ForMember(dest => dest.IssueId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority))
                .ForMember(dest => dest.UserId ,opt =>opt.MapFrom(src=> src.ReporterId))
                .ForMember(dest => dest.Title , opt => opt.MapFrom(src =>src.Title))
                .ForMember(dest=>dest.Description , opt => opt.MapFrom(src=> src.Description))
                .ForMember(dest => dest.ImageUrl , opt => opt.MapFrom(src => src .IssueAttachments.Select(x=>x.Url)))
                .ForMember(dest => dest .CreatedAt , opt => opt.MapFrom(src => src.CreatedAt));


            CreateMap<Issue.Domain.Entities.Issue.Issue, AssignExpertResponse>()
               .ForMember(dest => dest.IssueId, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.AssignedExpertId, opt => opt.MapFrom(src => src.AssignedExpertId))
               .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));


        }
    }
}

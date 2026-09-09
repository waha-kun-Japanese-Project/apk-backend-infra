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
            CreateMap<Issue.Domain.Entities.Issue.Issue,AssignExpertResponse>()
                .ForMember(dest => dest.IssueId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.AssignedExpertId, opt => opt.MapFrom(src => src.AssignedExpertId))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));


        }
    }
}

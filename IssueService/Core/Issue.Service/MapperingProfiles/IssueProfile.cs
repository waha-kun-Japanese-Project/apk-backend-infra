using AutoMapper;
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
            CreateMap<Domain.Entities.Issue.Issue, Shared.DTOS.ReturnIssueByIdForFarmer>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority));
                

        }
    }
}

using AutoMapper;
using Issue.Domain.Entities.Issue;
using Issue.Shared.DTOS;
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

            CreateMap<Issue.Domain.Entities.Issue.Issue, ExpertInboxResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(opt => opt.Description))
                .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(opt => opt.Status.ToString()))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest=>dest.AssignedExpertId,opt=>opt.MapFrom(src=>src.AssignedExpertId));
              


            CreateMap <Issue.Domain.Entities.Issue.Issue, CaseReviewResponse>()
                .ForMember(dest => dest.Status,
                      opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Priority,
                      opt => opt.MapFrom(src => src.Priority.ToString()))
                .ForMember(dest => dest.Latitude,
                      opt => opt.MapFrom(src => src.GPSLocation.Latitude.ToString()))
               .ForMember(dest => dest.Longitude,
                      opt => opt.MapFrom(src => src.GPSLocation.Longitude.ToString()))
               .ForMember(dest => dest.Attachments,
                      opt => opt.MapFrom(src => src.IssueAttachments))
              .ForMember(dest => dest.AiAnalysis,
                      opt => opt.MapFrom(src => src.AiAnalyses))
              .ForMember(dest => dest.ExpertReviews,
                      opt => opt.MapFrom(src => src.ExpertReviews));


            CreateMap<IssueAttachment, IssueAttachmentResponse>();

            CreateMap<AiAnalysis, AiAnalysisSummary>();

            CreateMap<ExpertReviews, ExpertReviewResponse>();

            CreateMap<ExpertReviews, SubmitExpertReviewResponse>()
                .ForMember(dest => dest.IssueId, opt => opt.MapFrom(src => src.IssueId))
                .ForMember(dest => dest.ReviewId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Issue.Status.ToString()))
                .ForMember(dest => dest.Decision, opt => opt.MapFrom(src => src.Decision.ToString()))
                .ForMember(dest =>dest.Notes,opt=>opt.MapFrom(src=>src.Notes));

            CreateMap<RepairSchedule, RepairScheduleResponse>();
               

        }
    }
}

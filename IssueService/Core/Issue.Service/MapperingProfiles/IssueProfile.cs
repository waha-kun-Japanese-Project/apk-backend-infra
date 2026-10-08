using AutoMapper;
using Issue.Domain.Entities.Issue;
using Issue.Service.Specifications.FarmerSpecifications;
using Issue.Shared.DTOS;
using Issue.Shared.DTOS.AssignExpert;
using Issue.Shared.DTOS.FarmerDtos;
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
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.ReporterId))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.IssueAttachments.Select(x => x.Url)))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));


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
              //.ForMember(dest => dest.AiAnalysis,
              //        opt => opt.MapFrom(src => src.AiAnalyses))
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

            CreateMap<Issue.Domain.Entities.Issue.Issue, Issue.Shared.DTOS.FarmerDtos.GetFarmerIssues>()
                 .ForMember(dest => dest.IssueId, opt => opt.MapFrom(src => src.Id))
                 .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                 .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                 .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                 .ForMember(dest => dest.longitude , opt => opt.MapFrom(src => src.GPSLocation.Longitude))
                 .ForMember(dest => dest.Latiude, opt => opt.MapFrom(src => src.GPSLocation.Latitude))
                 .ForMember(dest=>dest.priority,opt=> opt.MapFrom(src => src.Priority))
                 .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
           

            CreateMap<StatusHistory, IssueTrackingStepDto>()
                .ForMember(
                    dest => dest.Status,
                    opt => opt.MapFrom(src => src.Status))
                .ForMember(
                    dest => dest.ChangedAt,
                    opt => opt.MapFrom(src => src.ChangedAt))
                .ForMember(
                    dest => dest.Note,
                    opt => opt.MapFrom(src => src.Note))
                .ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(src => GetStatusName(src.Status).ToString()));

            CreateMap<StatusHistory,CompleteStausReponse>()
                .ForMember(dest => dest.status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.IssueId, opt => opt.MapFrom(src => src.IssueId))
                .ForMember(dest => dest.ChangedAt , opt => opt.MapFrom(src => src.ChangedAt))
                .ForMember(dest => dest.ReporterId , opt=> opt.MapFrom(src=>src.ChangedById));



        }
        private static string GetStatusName(IssueStatus status)
        {
            return status switch
            {
                IssueStatus.Reported => "Issue Reported",
                IssueStatus.Diagnosed => "AI Diagnosis",
                IssueStatus.Reviewed => "Expert Review",
                IssueStatus.Assigned => "Expert Assigned",
                IssueStatus.Scheduled => "Repair Scheduled",
                IssueStatus.Repaired => "Repair Completed",
                IssueStatus.completed => "Follow-up and Confirmation",
                _ => status.ToString()
            };
        }
    }
}

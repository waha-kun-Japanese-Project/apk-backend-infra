using AutoMapper;
using Report.Domain.Entities.Issue;
using Report.Shared.DTOS.Report;

namespace Report.Service.Mapping.Profile
{
    public class IssueProfile : AutoMapper.Profile
    {
        public IssueProfile()
        {
            CreateMap<Issue, CreateIssueResponse>()
                .ConvertUsing(src => new CreateIssueResponse(
                    src.Id,
                    src.Description,
                    src.Status,
                    src.CreatedAt,
                    src.ReporterId
                ));

            CreateMap<AiAnalysisResponse, AiAnalysis>()
                .ForMember(
                    dest => dest.ModelVersion,
                    opt => opt.MapFrom(_ => string.Empty));

            CreateMap<CreateIssueRequest, Issue>()
                .ForMember(
                    dest => dest.ReporterId,
                    opt => opt.Ignore())
                .ForMember(
                    dest => dest.Status,
                    opt => opt.MapFrom(_ => IssueStatus.Diagnosed))
                .ForMember(
                    dest => dest.GPSLocation,
                    opt => opt.MapFrom(src =>
                        new GPSLocation
                        {
                            Latitude = src.Latitude,
                            Longitude = src.Longitude
                        }))
                .ForMember(
                    dest => dest.IssueAttachments,
                    opt => opt.Ignore())
                .ForMember(
                    dest => dest.AiAnalyses,
                    opt => opt.Ignore());
        }
    }
}
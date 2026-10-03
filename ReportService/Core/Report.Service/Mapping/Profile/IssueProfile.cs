
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
                .ForCtorParam("Id",
                    opt => opt.MapFrom(src => src.Id))
                .ForCtorParam("Description",
                    opt => opt.MapFrom(src => src.Description))
                .ForCtorParam("Status",
                    opt => opt.MapFrom(src => src.Status))
                .ForCtorParam("CreatedAt",
                    opt => opt.MapFrom(src => src.CreatedAt))
                .ForCtorParam("ReporterId",
                    opt => opt.MapFrom(src => src.ReporterId));

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
                ;
        }
    }
}
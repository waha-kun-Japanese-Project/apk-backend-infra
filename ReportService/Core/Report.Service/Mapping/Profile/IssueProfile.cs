//using AutoMapper;
//using Report.Domain.Entities.Issue;
//using Report.Shared.DTOS.Client;
//using Report.Shared.DTOS.Report;
//using System.Linq;

//namespace Report.Service.Mapping.Profile
//{
//    public class IssueProfile : AutoMapper.Profile
//    {
//        public IssueProfile()

//        {
//            CreateMap<Issue, CreateIssueResponse>();
//            CreateMap<CreateIssueRequest, Issue>()
//           .ForMember(dest => dest.ReporterId,
//               opt => opt.Ignore())
//           .ForMember(dest => dest.Status,
//               opt => opt.MapFrom(_ => IssueStatus.Diagnosed))
//          .ForMember(  dest => dest.GPSLocation, opt => opt.MapFrom(src =>
//                  new GPSLocation
//                     {
//                        Latitude = src.Latitude,
//                       Longitude = src.Longitude
//                     }))

//           .ForMember(dest => dest.IssueAttachments,
//               opt => opt.Ignore())
//           .ForMember(dest => dest.AiAnalyses,
//               opt => opt.Ignore());


//            CreateMap<AiPredictionResponse, AiAnalysis>()
//               .ForMember(dest => dest.ProblemName, opt => opt.MapFrom(src => src.ProblemCode ?? string.Empty))
//                .ForMember(dest => dest.ProblemArabic, opt => opt.MapFrom(src => src.Problem ?? string.Empty))
//                .ForMember(dest => dest.Confidence,
//                    opt => opt.MapFrom(src =>
//                        AiAnalysisMapper.ParseConfidence(src.Confidence)))
//                .ForMember(dest => dest.Severity,
//                    opt => opt.MapFrom(src =>
//                       src.Severity))
//                .ForMember(dest => dest.Recommendation,
//                    opt => opt.MapFrom(src => src.Recommendation ?? string.Empty))
//                .ForMember(dest => dest.Explanation,
//                    opt => opt.MapFrom(src => src.Explanation ?? string.Empty))
//                .ForMember(dest => dest.RepairSteps,
//                    opt => opt.MapFrom(src =>
//                        src.RepairSteps ?? new List<string>()))
//                .ForMember(dest => dest.ModelVersion,
//                    opt => opt.MapFrom(_ => string.Empty));
//        }
//    }   
//}
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
                .ForMember(
                    dest => dest.AiAnalyses,
                    opt => opt.Ignore());
        }
    }
}
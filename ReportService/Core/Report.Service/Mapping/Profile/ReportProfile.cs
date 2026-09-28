using AutoMapper;
using Microsoft.AspNetCore.Http;
using Report.Domain.Entities.Report;
using Report.Shared.DTOS.Client;
using Report.Shared.DTOS.Report;
using System.Linq;

namespace Report.Service.Mapping.Profile
{
        public class ReportAttachmentConverter(IHttpContextAccessor httpContextAccessor)
        : ITypeConverter<ReportAttachment, ReportAttachmentResponse>
    {
        private const int MinioNodePort = 30900;

        public ReportAttachmentResponse Convert(ReportAttachment source, ReportAttachmentResponse destination, ResolutionContext context)
        {
            var request = httpContextAccessor.HttpContext?.Request;
            var url = request is null
                ? source.Url
                : $"{request.Scheme}://{request.Host.Host}:{MinioNodePort}/reportimage/{source.Url}";

            return new ReportAttachmentResponse(
                source.Id,
                source.Type.ToString(),
                url,
                source.CreatedAt);
        }
    }

    public class ReportProfile : AutoMapper.Profile
    {
        public ReportProfile()
        {
            CreateMap<Report.Domain.Entities.Report.Report, ReportDetailsResponse>()
                .ForCtorParam("Id",
                    opt => opt.MapFrom(src => src.Id))
                .ForCtorParam("Status",
                    opt => opt.MapFrom(src => src.Status.ToString()))
                .ForCtorParam("Latitude",
                    opt => opt.MapFrom(src => src.Location != null ? (string?)src.Location.Latitude : null))
                .ForCtorParam("Longitude",
                    opt => opt.MapFrom(src => src.Location != null ? (string?)src.Location.Longitude : null))
                .ForCtorParam("Attachments",
                    opt => opt.MapFrom(src => src.Attachments))
                .ForCtorParam("Analysis",
                    opt => opt.MapFrom(src => src.Analysis));
            CreateMap<Report.Domain.Entities.Report.Report, CreateReportResponse>().ForCtorParam("Id",
                    opt => opt.MapFrom(src => src.Id))
                .ForCtorParam("Status",
                    opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<ReportAttachment, ReportAttachmentResponse>()
                .ConvertUsing<ReportAttachmentConverter>();

            CreateMap<AiAnalysis, AiAnalysisResponse>()
                .ForCtorParam("Severity",
                    opt => opt.MapFrom(src => src.Severity.ToString()));
        }
    }
}
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Report.Domain.Entities.Report;
using Report.Shared.DTOS.Client;
using Report.Shared.DTOS.Report;
using System.Collections.Generic;
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
            // Every destination below is a `record`. Records get a synthesized public
            // copy-constructor in addition to the primary one, and ForCtorParam can end up
            // targeting the wrong constructor when there are two to choose from, throwing
            // "does not have a matching constructor with a parameter named 'X'" even though
            // the name genuinely exists on the primary constructor. ConvertUsing sidesteps
            // that entirely by calling the constructor directly — no ambiguity possible.

            CreateMap<Report.Domain.Entities.Report.Report, ReportDetailsResponse>()
                .ConvertUsing((src, dest, context) => new ReportDetailsResponse(
                    src.Id,
                    src.Description,
                    src.Status.ToString(),
                    src.CreatedAt,
                    src.UpdatedAt,
                    src.ReporterId,
                    src.Location != null ? src.Location.Latitude : null,
                    src.Location != null ? src.Location.Longitude : null,
                    context.Mapper.Map<List<ReportAttachmentResponse>>(src.Attachments),
                    src.Analysis != null ? context.Mapper.Map<AiAnalysisResponse>(src.Analysis) : null
                ));

            CreateMap<Report.Domain.Entities.Report.Report, CreateReportResponse>()
                .ConvertUsing(src => new CreateReportResponse(
                    src.Id,
                    src.Description,
                    src.Status.ToString(),
                    src.CreatedAt,
                    src.UpdatedAt,
                    src.ReporterId
                ));

            CreateMap<ReportAttachment, ReportAttachmentResponse>()
                .ConvertUsing<ReportAttachmentConverter>();

            CreateMap<AiAnalysis, AiAnalysisResponse>()
                .ConvertUsing(src => new AiAnalysisResponse(
                    string.Empty,
                    src.ProblemName,
                    src.ProblemArabic,
                    src.Confidence,
                    src.Severity.ToString(),
                    src.Recommendation,
                    src.Explanation,
                    src.RepairSteps
                ));
        }
    }
}
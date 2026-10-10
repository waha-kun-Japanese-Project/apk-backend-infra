using AutoMapper;
using Report.Domain.Entities.Issue;
using Report.Shared.DTOS.Client;
using Report.Shared.DTOS.Report;
using System;
using System.Net.Mail;

namespace Report.Service.Mapping;

public class IssueProfile : Profile
{
    public IssueProfile()
    {
        // AI Prediction → AI Analysis
        CreateMap<AiPredictionResponse, AiAnalysis>()
            .ForMember(
                dest => dest.ProblemName,
                opt => opt.MapFrom(src => src.Problem))
            .ForMember(
                dest => dest.ProblemArabic,
                opt => opt.MapFrom(src => src.Problem))
            .ForMember(
                dest => dest.Confidence,
                opt => opt.MapFrom(src =>
                    AiAnalysisMapper.ParseConfidence(src.Confidence)))
            .ForMember(
                dest => dest.Severity,
                opt => opt.MapFrom(src => src.Severity ?? "Unknown"))
            .ForMember(
                dest => dest.Recommendation,
                opt => opt.MapFrom(src => src.Recommendation ?? string.Empty))
            .ForMember(
                dest => dest.RepairSteps,
                opt => opt.MapFrom(src =>
                    src.RepairSteps ?? new List<string>()))
            .ForMember(
                dest => dest.IssueAttachment,
                opt => opt.Ignore());


        // FilePath → IssueAttachment
        CreateMap<string, IssueAttachment>()
            .ForMember(
                dest => dest.Url,
                opt => opt.MapFrom(src => src))
            .ForMember(
                dest => dest.Type,
                opt => opt.MapFrom(_ => IssueAttachmentType.Photo))
            .ForMember(
                dest => dest.Purpose,
                opt => opt.MapFrom(_ =>
                    IssueAttachmentPurpose.ProblemReport));


        // AI Prediction → Response
        CreateMap<AiPredictionResponse, AiAnalysisResponse>()
            .ForMember(
                dest => dest.FilePath,
                opt => opt.Ignore())
            .ForMember(
                dest => dest.ProblemName,
                opt => opt.MapFrom(src => src.Problem))
            .ForMember(
                dest => dest.ProblemArabic,
                opt => opt.MapFrom(src => src.Problem))
            .ForMember(
                dest => dest.Confidence,
                opt => opt.MapFrom(src =>
                    AiAnalysisMapper.ParseConfidence(src.Confidence)))
            .ForMember(
                dest => dest.RepairSteps,
                opt => opt.MapFrom(src =>
                    src.RepairSteps ?? new List<string>()));


        // CreateIssueRequest → Issue
        CreateMap<CreateIssueRequest, Issue>()
            .ForMember(
                dest => dest.Status,
                opt => opt.MapFrom(_ => IssueStatus.Diagnosed));
    }
}

using AutoMapper;
using Map.Domain.Entities.ISSUE;
using Map.Shared;
using System.Data.SqlTypes;

namespace Map.Service.Mapping.Profile
{
    public class MapProfile : AutoMapper.Profile
    {
        public MapProfile()
        {
            CreateMap<Issue, MapResponseDto>()

                // Id
                .ForMember(
                    dest => dest.IssueId,
                    opt => opt.MapFrom(src => src.Id)
                )

                // GPS
                .ForMember(
                    dest => dest.Longitde,
                    opt => opt.MapFrom(src =>
                        src.GPSLocation != null
                            ? src.GPSLocation.Longitude
                            : string.Empty)
                )

                .ForMember(
                    dest => dest.Latitude,
                    opt => opt.MapFrom(src =>
                        src.GPSLocation != null
                            ? src.GPSLocation.Latitude
                            : string.Empty)
                )

             // Photo
            .ForMember(
                    dest => dest.PhotoUrl,
                    opt => opt.MapFrom(src =>
                        src.IssueAttachments
                            .Select(x => x.Url)
                            .FirstOrDefault()
                    )
                )
                

                // CreatedAt
                .ForMember(
                    dest => dest.CreatedAt,
                    opt => opt.MapFrom(src => DateTime.SpecifyKind(src.CreatedAt, DateTimeKind.Utc))
               )

                // Priority: enum -> string
                .ForMember(
                    dest => dest.priority,
                    opt => opt.MapFrom(src => src.Priority.ToString())
                )

                // Status: enum -> string
                .ForMember(
                    dest => dest.Status,
                    opt => opt.MapFrom(src => src.Status.ToString())
                )

                // Title
                .ForMember(
                    dest => dest.title,
                    opt => opt.MapFrom(src => src.Title)
                );
        }
    }
}
using AutoMapper;
using LogTransformer.Core.Entities;
using LogTransformer.Api.Models;

namespace LogTransformer.Api.Mapping
{
    public class MappingProfileGet : Profile
    {
        public MappingProfileGet()
        {
            CreateMap<LogEntry, LogResponseDto>();
        }
    }
}

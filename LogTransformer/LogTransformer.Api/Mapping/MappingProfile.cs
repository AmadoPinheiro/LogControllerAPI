using AutoMapper;
using LogTransformer.Api.Models;
using LogTransformer.Core.Entities;

namespace LogTransformer.Api.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<LogEntryDto, LogEntry>();
        }
    }
}

using AutoMapper;
using LogTransformer.Api.Models;
using LogTransformer.Core.Entities;

namespace LogTransformer.Api.Mappings
{
    public class MappingProfilePost : Profile
    {
        public MappingProfilePost()
        {
            CreateMap<LogEntryDto, LogEntry>();
        }
       
    }
}

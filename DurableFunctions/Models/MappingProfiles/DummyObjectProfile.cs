using AutoMapper;
using DurableFunctions.Models.Dto;

namespace DurableFunctions.Models.MappingProfiles
{
    public class DummyObjectProfile : Profile
    {
        public DummyObjectProfile()
        {
            CreateMap<DummyObject, DummyObjectTableEntity>().ForMember(t => t.RowKey, f => f.MapFrom(src => src.UniqueID));
        }
    }
}

using AutoMapper;
using CatApi.Models.Entities;
using CatApi.Models.Response;

namespace CatApi.MapperProfiles;


public class MappingEntityCatProfile : Profile
{
    public MappingEntityCatProfile()
    {
        CreateMap<CatEntity, CatResponse>()
            .ForMember(dest => dest.Tags,
                       opt => opt.MapFrom(src => src.CatTags.Select(t => t.Tag).Select(t => t.Name).OrderBy(n => n).ToList()));
    }
}


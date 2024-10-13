using AutoMapper;
using CatApi.Helpers;
using CatApi.Models.Entities;
using CatApi.Models.SourceApi;
using CatApi.Repositories.Interfaces;

namespace CatApi.MapperProfiles;

public class MappingSourceCatProfile : Profile
{
    public MappingSourceCatProfile()
    {        

        CreateMap<CatSourceResponse, CatEntity>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CatId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Height, opt => opt.MapFrom(src => src.Height))
            .ForMember(dest => dest.Width, opt => opt.MapFrom(src => src.Width))
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Url));

    }
}

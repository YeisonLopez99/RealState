using Application.Products.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public class PropertyProfile : Profile
    {
        public PropertyProfile()
        {
            CreateMap<Property, PropertyDto>()
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images));

            CreateMap<PropertyImage, PropertyImageDto>();
        }
    }
}

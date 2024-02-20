using AutoMapper;
using SimpleECommerce.Core.Dtos;
using SimpleECommerce.Core.Entities;

namespace SimpleECommerce.Core.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<CategoryCreateDto, Category>()
           // Exclude Image property as it's handled separately
           .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
           .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true)); // Set default IsActive to true

            CreateMap<CategoryDto, Category>().ReverseMap();
            CreateMap<CategoryUpdateDto, Category>().ReverseMap();



            CreateMap<ProductDto, Product>().ReverseMap().ForMember(dest => dest.Images, opt => opt.Ignore());
            CreateMap<ProductCreateDto, Product>().ReverseMap(); 
            CreateMap<ProductUpdateDto, Product>().ReverseMap();

        }

    }
}

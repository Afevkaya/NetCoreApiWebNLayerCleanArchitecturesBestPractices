using App.Repositories.Categories;
using App.Services.Categories.Create;
using App.Services.Categories.Response;
using App.Services.Categories.Update;
using AutoMapper;

namespace App.Services.Categories;

public class CategoryMappingProfile: Profile
{
    public CategoryMappingProfile()
    {
        CreateMap<Category, CategoryResponse>().ReverseMap();
        CreateMap<Category, CategoryWithProductsResponse>();
        CreateMap<CategoryCreateRequest, Category>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.ToLowerInvariant()));
        CreateMap<CategoryUpdateRequest,Category>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.ToLowerInvariant()));
    }
}
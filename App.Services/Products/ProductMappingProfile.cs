using App.Repositories.Products;
using App.Services.Products.Create;
using App.Services.Products.Response;
using App.Services.Products.Update;
using AutoMapper;

namespace App.Services.Products;

public class ProductMappingProfile: Profile
{
    public ProductMappingProfile()
    {
        CreateMap<Product, ProductResponse>().ReverseMap();
        CreateMap<ProductCreateRequest, Product>()
            .ForMember(dest=>dest.Name,opt=>opt.MapFrom(src=>src.Name.ToLowerInvariant()))
            .ForMember(dest=>dest.Stock,opt=>opt.MapFrom(src=>src.Quantity));
        CreateMap<ProductUpdateRequest, Product>()
            .ForMember(dest=>dest.Name,opt=>opt.MapFrom(src=>src.Name.ToLowerInvariant()))
            .ForMember(dest=>dest.Stock,opt=>opt.MapFrom(src=>src.Quantity));
    }
}
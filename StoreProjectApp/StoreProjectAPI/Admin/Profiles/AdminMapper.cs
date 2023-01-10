using AutoMapper;
using StoreProjectAPI.Admin.Dtos.CategoryDtos;
using StoreProjectAPI.Admin.Dtos.ProductDtos;
using Store.Core.Entities;

namespace StoreProjectAPI.Admin.Profiles
{
    public class AdminMapper:Profile
    {
        private readonly IHttpContextAccessor _httpAccessor;

        public AdminMapper(IHttpContextAccessor httpAccessor)
        {
            _httpAccessor = httpAccessor;

            CreateMap<Category, CategoryGetDto>();
            CreateMap<CategoryPostDto, Category>();
            CreateMap<Category, CategoryListItemDto>();
            CreateMap<Category, CategoryInProductGetDto>();

            CreateMap<ProductPostDto, Product>();
            CreateMap<Product, ProductGetDto>()
                .ForMember(x => x.DiscountedPrice, f => f.MapFrom(s => s.SalePrice * (100 - s.DiscountPercent) / 100))
                .ForMember(x => x.ImageURL, f => f.MapFrom(s => $"{_httpAccessor.HttpContext.Request.Scheme}://{_httpAccessor.HttpContext.Request.Host}{_httpAccessor.HttpContext.Request.PathBase}/uploads/products/{s.Image}"));
            CreateMap<Product, ProductListItemDto>();

        }
    }
}

using AutoMapper;
using Store.Core.Entities;
using StoreProjectAPI.Member.Dtos.CategoryDtos;
using StoreProjectAPI.Member.Dtos.ProductDtos;

namespace StoreProjectAPI.Member.Profiles
{
    public class MemberMapper : Profile
    {
        public MemberMapper()
        {
            CreateMap<Category, CategoryGetDto>();
            CreateMap<Category, CategoryListItemDto>();
            CreateMap<Category, CategoryInProductGetDto>();

            CreateMap<Product, ProductGetDto>()
               .ForMember(x => x.DiscountedPrice, f => f.MapFrom(s => s.SalePrice * (100 - s.DiscountPercent) / 100));
            CreateMap<Product, ProductListItemDto>();
        }
    }
}

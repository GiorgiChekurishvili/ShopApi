using AutoMapper;
using WebApiWithoutEmpty.Dtos;
using WebApiWithoutEmpty.Entities;

namespace WebApiWithoutEmpty
 
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<ProductReturnDto, Product>();
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, CategoryReturnDto>();
            CreateMap<User, UserForLoginDto>().ReverseMap();
            CreateMap<User, UserForRegisterDto>().ReverseMap();
        }
    }
}

using AutoMapper;
using ComputerStore.Application.DTOs;
using ComputerStore.Domain.Entities;

namespace ComputerStore.API.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();

        }
    }
}

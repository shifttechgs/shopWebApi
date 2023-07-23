using AutoMapper;
using ShopWebApi.DTOs;
using ShopWebApi.Entities;
using ShopWebApi.Models;

namespace ShopWebApi.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterRequest, User>();
            CreateMap<ProductRequest, Product>();
        }
    }
}
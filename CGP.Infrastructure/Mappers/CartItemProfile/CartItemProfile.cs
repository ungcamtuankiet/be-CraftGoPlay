using AutoMapper;
using CGP.Contract.DTO.CartItem;
using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Infrastructure.Mappers.CartItemProfile
{
    public class CartItemProfile : Profile
    {
        public CartItemProfile()
        {
            CreateMap<CartItem, CartItemDto>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
            .ForMember(dest => dest.ProductImages, opt => opt.MapFrom(src => src.Product.ProductImages))
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.Product.User))
            .ReverseMap();

            CreateMap<AddCartItemDto, CartItem>()
            .ForMember(dest => dest.UnitPrice, opt => opt.Ignore())
            .ForMember(dest => dest.AddedAt, opt => opt.Ignore()).ReverseMap();
        }
    }
}

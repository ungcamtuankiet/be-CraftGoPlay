using AutoMapper;
using CGP.Contract.DTO.ShopPrice;
using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Infrastructure.Mappers.ShopPriceProfile
{
    public class ShopPriceProfile : Profile
    {
        public ShopPriceProfile()
        {
            CreateMap<ShopPrice, CreateItemShopPriceDTO>().ReverseMap();
            CreateMap<ShopPrice, UpdateItemShopPriceDTO>().ReverseMap();
            CreateMap<ShopPrice, ViewItemShopPriceDTO>()
                .ForMember(dest => dest.Item, opt => opt.MapFrom(src => src.Item))
                .ReverseMap();

            CreateMap<ShopPrice, ViewItemsSeillDTO>()
                .ForMember(dest => dest.SellPrice, opt => opt.MapFrom(src => src.Price))
                .ReverseMap();
        }
    }
}

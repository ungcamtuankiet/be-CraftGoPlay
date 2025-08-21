using AutoMapper;
using CGP.Contract.DTO.Order;
using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Infrastructure.Mappers.OrderProfile
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, ViewOrderDTO>()
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems))
                .ForMember(dest => dest.UserAddress, opt => opt.MapFrom(src => src.OrderAddress))
                .ReverseMap();

            CreateMap<Order, CreateDirectOrderDto>().ReverseMap();
            
            CreateMap<Order, CreateOrderFromCartDto>().ReverseMap();
        }
    }
}

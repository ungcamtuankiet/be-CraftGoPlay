using AutoMapper;
using CGP.Contract.DTO.OrderItem;
using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Infrastructure.Mappers.OrderItemProfile
{
    public class OrderItermProfile : Profile
    {
        public OrderItermProfile()
        {
            CreateMap<OrderItem, ViewOrderItemDTO>()
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product))
                .ReverseMap();
        }
    }
}

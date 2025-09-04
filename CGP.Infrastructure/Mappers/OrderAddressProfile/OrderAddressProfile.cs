using AutoMapper;
using CGP.Contract.DTO.OrderAddress;
using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Infrastructure.Mappers.OrderAddressProfile
{
    public class OrderAddressProfile : Profile
    {
        public OrderAddressProfile()
        {
            CreateMap<OrderAddress, ViewOrderAddressDTO>().ReverseMap();
        }
    }
}

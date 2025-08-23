using AutoMapper;
using CGP.Contract.DTO.RefundRequest;
using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Infrastructure.Mappers.ReturnRequestProfile
{
    public class ReturnRequestProfile : Profile
    {
        public ReturnRequestProfile()
        {
            CreateMap<ReturnRequest,SendRefundRequestDTO>().ReverseMap();
            CreateMap<ReturnRequest,ViewReturnRequestDTO>()
                .ForMember(rr => rr.User, opt => opt.MapFrom(src => src.User))
                .ForMember(rr => rr.OrderItem, opt => opt.MapFrom(src => src.OrderItem))
                .ReverseMap();
            CreateMap<ReturnRequest,ViewEscalatedDTO>()
                .ForMember(rr => rr.User, opt => opt.MapFrom(src => src.User))
                .ForMember(rr => rr.OrderItem, opt => opt.MapFrom(src => src.OrderItem))
                .ReverseMap();
        }
    }
}

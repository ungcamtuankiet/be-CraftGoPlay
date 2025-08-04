using AutoMapper;
using CGP.Contract.DTO.Point;
using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Infrastructure.Mappers.PointProfile
{
    public class PointProfile : Profile
    {
        public PointProfile()
        {
            CreateMap<Point, ViewPointDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.PointTransactions, opt => opt.MapFrom(src => src.PointTransactions))
                .ReverseMap();
        }
    }
}

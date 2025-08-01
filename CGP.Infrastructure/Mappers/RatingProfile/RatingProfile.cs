using AutoMapper;
using CGP.Contract.DTO.Rating;
using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Infrastructure.Mappers.RatingProfile
{
    public class RatingProfile : Profile
    {
        public RatingProfile()
        {
            CreateMap<Rating, RatingDTO>().ReverseMap();
            CreateMap<Rating, ViewRatingDTO>().ReverseMap();
            CreateMap<Rating, ViewRatingProductDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ReverseMap();
        }
    }
}

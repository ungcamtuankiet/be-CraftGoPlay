using AutoMapper;
using CGP.Contract.DTO.User;
using CGP.Contract.DTO.UserVoucher;
using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Infrastructure.Mappers.UserProfile
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDTO, ApplicationUser>().ReverseMap();
            CreateMap<ViewUserReturnRequestDTO, ApplicationUser>().ReverseMap();
            CreateMap<ViewArtisanInfoCartitemDTO, ApplicationUser>().ReverseMap();
            CreateMap<CreateNewAccountDTO, ApplicationUser>().ReverseMap();
            CreateMap<UpdateAccountDTO, ApplicationUser>().ReverseMap();
            CreateMap<ArtisanDTO, ApplicationUser>().ReverseMap();
            CreateMap<ViewUserVoucherDTO, ApplicationUser>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId))
                .ReverseMap();
        }
    }
}

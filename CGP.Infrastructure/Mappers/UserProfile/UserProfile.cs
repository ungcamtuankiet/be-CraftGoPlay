using AutoMapper;
using CGP.Contract.DTO.User;
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
            CreateMap<CreateNewAccountDTO, ApplicationUser>().ReverseMap();
        }
    }
}

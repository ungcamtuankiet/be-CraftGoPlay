using AutoMapper;
using CGP.Contract.DTO.UserAddress;
using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Infrastructure.Mappers.UserAddressProfile
{
    public class UserAddressProfile : Profile
    {
        public UserAddressProfile()
        {
            CreateMap<UserAddress, ViewAddressDTO>().ReverseMap();

            CreateMap<UserAddress, AddNewAddressDTO>().ReverseMap();

            CreateMap<UserAddress, UpdateAddressDTO>().ReverseMap();
        }
    }
}

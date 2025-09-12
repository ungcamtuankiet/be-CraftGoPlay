using AutoMapper;
using CGP.Contract.DTO.UserVoucher;
using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Infrastructure.Mappers.UserVoucherProfile
{
    public class UserVoucherProfile : Profile
    {
        public UserVoucherProfile()
        {
            CreateMap<UserVoucher, ViewUserVoucherDTO>();
        }
    }
}

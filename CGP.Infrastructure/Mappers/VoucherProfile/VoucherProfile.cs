using AutoMapper;
using CGP.Contract.DTO.Voucher;
using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Infrastructure.Mappers.VoucherProfile
{
    public class VoucherProfile : Profile
    {
        public VoucherProfile()
        {
            CreateMap<Voucher, ViewVoucherDTO>().ReverseMap();
            CreateMap<Voucher, CreateVoucherDTO>().ReverseMap();
            CreateMap<Voucher, UpdateVoucherDTO>().ReverseMap();
        }
    }
}

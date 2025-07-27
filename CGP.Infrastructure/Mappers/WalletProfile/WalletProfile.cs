using AutoMapper;
using CGP.Contract.DTO.Wallet;
using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Infrastructure.Mappers.WalletProfile
{
    public class WalletProfile : Profile
    {
        public WalletProfile()
        {
            CreateMap<Wallet, ViewWalletDTO>().ReverseMap();
        }
    }
}

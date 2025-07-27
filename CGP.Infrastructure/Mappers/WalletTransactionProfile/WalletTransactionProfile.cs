using AutoMapper;
using CGP.Contract.DTO.WalletTransaction;
using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Infrastructure.Mappers.WalletTransactionProfile
{
    public class WalletTransactionProfile : Profile
    {
        public WalletTransactionProfile()
        {
            CreateMap<WalletTransaction, ViewWalletTransactionDTO>()
                .ForMember(dest => dest.DateTransaction, opt => opt.MapFrom(src => src.CreatedAt))
                .ReverseMap();
        }
    }
}

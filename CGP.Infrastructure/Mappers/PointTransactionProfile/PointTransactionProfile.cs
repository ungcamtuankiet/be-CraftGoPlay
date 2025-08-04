using AutoMapper;
using CGP.Contract.DTO.PointTransaction;
using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Infrastructure.Mappers.PointTransactionProfile
{
    public class PointTransactionProfile : Profile
    {
        public PointTransactionProfile()
        {
            CreateMap<PointTransaction, ViewPointTransactionDTO>().ReverseMap();
        }
    }
}

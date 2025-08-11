using AutoMapper;
using CGP.Contract.DTO.Transaction;
using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Infrastructure.Mappers.TransactionProfile
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            CreateMap<Transaction, ViewTransactionDTO>().ReverseMap();
        }
    }
}

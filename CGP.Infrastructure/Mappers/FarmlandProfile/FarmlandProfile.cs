using AutoMapper;
using CGP.Contract.DTO.Farmland;
using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Infrastructure.Mappers.FarmlandProfile
{
    public class FarmlandProfile : Profile
    {
        public FarmlandProfile()
        {
            CreateMap<FarmLand, ViewFarmlandDTO>()
                .ReverseMap();
        }
    }
}

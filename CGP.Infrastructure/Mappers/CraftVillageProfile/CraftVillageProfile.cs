using AutoMapper;
using CGP.Contract.DTO.CraftVillage;
using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Infrastructure.Mappers.CraftVillageProfile
{
    public class CraftVillageProfile : Profile
    {
        public CraftVillageProfile()
        {
            CreateMap<CraftVillage, ViewCraftVillageDTO>().ReverseMap();
        }
    }
}

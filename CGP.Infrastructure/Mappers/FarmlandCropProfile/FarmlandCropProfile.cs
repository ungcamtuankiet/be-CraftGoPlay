using AutoMapper;
using CGP.Contract.DTO.FarmlandCrop;
using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Infrastructure.Mappers.FarmlandCropProfile
{
    public class FarmlandCropProfile : Profile
    {
        public FarmlandCropProfile()
        {
            CreateMap<FarmlandCrop, ViewFarmlandCropDTO>()
                .ReverseMap();
        }
    }
}

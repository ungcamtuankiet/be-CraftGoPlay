using AutoMapper;
using CGP.Contract.DTO.Crop;
using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Infrastructure.Mappers.CropProfile
{
    public class CropProfile : Profile
    {
        public CropProfile()
        {
            CreateMap<Crop, ViewCropDTO>().ReverseMap();
            CreateMap<PlantCropDTO, Crop>().ReverseMap();
            CreateMap<UpdateCropDTO, Crop>().ReverseMap();
        }
    }
}

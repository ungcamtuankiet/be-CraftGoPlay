using AutoMapper;
using CGP.Contract.DTO.Meterial;
using CGP.Contract.DTO.Product;
using CGP.Domain.Entities;

namespace CGP.Infrastructure.Mappers.MeterialProfile
{
    public class MeterialProfile : Profile
    {
        public MeterialProfile()
        {
            CreateMap<MeterialCreateDTO, Meterial>()
               .ReverseMap();

            CreateMap<MeterialUpdateDTO, Meterial>().ReverseMap();

            CreateMap<Meterial, ViewMeterialDTO>()
               .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products))
               .ReverseMap();

            CreateMap<MeterialUpdateDTO, ViewMeterialDTO>().ReverseMap();

            CreateMap<Product, ProductDTO>();
        }
    }
}

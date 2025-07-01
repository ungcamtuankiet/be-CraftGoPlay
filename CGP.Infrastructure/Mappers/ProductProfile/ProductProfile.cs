using AutoMapper;
using CGP.Contract.DTO.Meterial;
using CGP.Contract.DTO.Product;
using CGP.Contract.DTO.ProductImage;
using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Infrastructure.Mappers.ProductProfile
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductCreateDto, Product>()
/*                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())*/
                .ForMember(dest => dest.Meterials, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<ProductUpdateDTO, Product>()
/*                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())*/
                .ForMember(dest => dest.Meterials, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<Product, ViewProductDTO>()
                    .ForMember(dest => dest.ArtisanName, opt => opt.MapFrom(src => src.User != null ? src.User.UserName : null))
                .ForMember(dest => dest.SubCategoryName, opt => opt.MapFrom(src => src.SubCategory != null ? src.SubCategory.SubName : null))
                .ForMember(dest => dest.Meterials, opt => opt.MapFrom(src => src.Meterials))
                .ForMember(dest => dest.ProductImages, opt => opt.MapFrom(src => src.ProductImages))
                .ReverseMap();

            CreateMap<Product, ProductDTO>().ReverseMap();
        }
    }
}

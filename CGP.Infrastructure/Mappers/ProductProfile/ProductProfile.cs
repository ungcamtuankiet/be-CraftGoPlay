using AutoMapper;
using CGP.Contract.DTO.Meterial;
using CGP.Contract.DTO.Product;
using CGP.Contract.DTO.ProductImage;
using CGP.Contract.DTO.Rating;
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
                .ForMember(dest => dest.Ratings, opt => opt.MapFrom(src => src.Ratings))
                .ForMember(dest => dest.TotalRatings, opt => opt.MapFrom(src => src.Ratings.Count))
                .ForMember(dest => dest.AverageRating,opt => opt.MapFrom(src => src.Ratings.Any() ? Math.Round(src.Ratings.Average(r => r.Star), 1) : 0))
                .ReverseMap();

            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.ArtisanName, opt => opt.MapFrom(src => src.User != null ? src.User.UserName : null))
                .ForMember(dest => dest.SubCategoryName, opt => opt.MapFrom(src => src.SubCategory != null ? src.SubCategory.SubName : null))
                .ForMember(dest => dest.ProductImages, opt => opt.MapFrom(src => src.ProductImages))
                .ReverseMap();
            CreateMap<Product, ViewProductFavouriteDTO>()
                .ForMember(dest => dest.ProductImages, opt => opt.MapFrom(src => src.ProductImages))
                .ReverseMap();
            CreateMap<Product, ViewProductOrderDTO>()
                .ForMember(dest => dest.ProductImages, opt => opt.MapFrom(src => src.ProductImages.FirstOrDefault()))
                .ReverseMap();

            CreateMap<Product, ViewProductBySubCategoryDTO>()
                .ForMember(dest => dest.SubId, opt => opt.MapFrom(src => src.SubCategoryId))
                .ForMember(dest => dest.SubName, opt => opt.MapFrom(src => src.SubCategory.SubName))
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.SubCategory.Products))
                .ReverseMap();

            CreateMap<Product, ViewRatingDTO>().ReverseMap();
        }
    }
}

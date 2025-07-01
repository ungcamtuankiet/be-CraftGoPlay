using AutoMapper;
using CGP.Contract.DTO.Product;
using CGP.Contract.DTO.ProductImage;
using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Infrastructure.Mappers.ProductImageProfile
{
    public class ProductImageProfile : Profile
    {
        public ProductImageProfile()
        {
            CreateMap<ProductImage, ProductImageDTO>().ReverseMap();
        }
    } 
}

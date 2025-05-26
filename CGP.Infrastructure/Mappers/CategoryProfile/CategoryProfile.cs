using AutoMapper;
using CGP.Contract.DTO.Category;
using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Infrastructure.Mappers.CategoryProfile
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<ViewCategoryDTO, Category>().ReverseMap()
                .ForMember(dest => dest.CategoryId, src => src.MapFrom(x => x.Id));
            CreateMap<UpdateCategoryDTO, Category>().ReverseMap();
            CreateMap<CreateCategoryWithSubDTO, Category>().ReverseMap();
            CreateMap<CreateCategoryDTO, Category>().ReverseMap();

            CreateMap<ViewSubCateDTO, SubCategory>().ReverseMap()
                .ForMember(p => p.SubId, b => b.MapFrom(m => m.Id));
        }
    }
}

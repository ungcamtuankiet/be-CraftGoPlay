using AutoMapper;
using CGP.Contract.DTO.Item;
using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Infrastructure.Mappers.ITemProfile
{
    public class ItemProfile : Profile
    {
        public ItemProfile()
        {
            CreateMap<Item, CreateItemDTO>().ReverseMap();
            CreateMap<Item, UpdateItemDTO>().ReverseMap();
            CreateMap<Item, ViewItemDTO>().ReverseMap();
        }
    }
}

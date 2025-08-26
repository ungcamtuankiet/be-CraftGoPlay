using AutoMapper;
using CGP.Contract.DTO.Inventory;
using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Infrastructure.Mappers.InventoryProfile
{
    public class InventoryProfile : Profile
    {
        public InventoryProfile()
        {
            CreateMap<Inventory, ViewInventoryDTO>()
                .ReverseMap();
            CreateMap<AddToInventoryDTO, Inventory>().ReverseMap();
            CreateMap<AddItemToInventoryDTO, Inventory>().ReverseMap();
            CreateMap<UpdateInventoryDTO, Inventory>().ReverseMap();
        }
    }
}

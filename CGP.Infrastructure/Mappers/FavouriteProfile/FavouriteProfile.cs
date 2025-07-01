using AutoMapper;
using CGP.Contract.DTO.Favourite;
using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Infrastructure.Mappers.FavouriteProfile
{
    public class FavouriteProfile : Profile
    {
        public FavouriteProfile() {
            CreateMap<ViewFavouriteDTO, Favourite>().ReverseMap();

            CreateMap<CreateFavouriteDTO, Favourite>().ReverseMap();
        }
    }
}

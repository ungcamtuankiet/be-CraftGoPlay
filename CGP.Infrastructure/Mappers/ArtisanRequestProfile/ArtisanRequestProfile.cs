using AutoMapper;
using CGP.Contract.DTO.ArtisanRequest;
using CGP.Domain.Entities;
using CGP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Infrastructure.Mappers.ArtisanRequestProfile
{
    public class ArtisanRequestProfile : Profile
    {
        public ArtisanRequestProfile()
        {
            CreateMap<SendRequestDTO, ArtisanRequest>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => RequestArtisanStatus.Pending));
            CreateMap<RejectRequestDTO, ArtisanRequest>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => RequestArtisanStatus.Rejected))
                .ForMember(dest => dest.Reason, opt => opt.MapFrom(src => src.Reason));
            CreateMap<ArtisanRequest, ViewRequestDTO>().ReverseMap();
            CreateMap<ArtisanRequest, ViewAddressOfArtisanDTO>().ReverseMap();
            CreateMap<ArtisanRequest, ViewArtisanRequestDTO>().ReverseMap();
        }
    }
}

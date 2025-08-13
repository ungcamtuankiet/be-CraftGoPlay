using AutoMapper;
using CGP.Contract.DTO.DailyCheckIn;
using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Infrastructure.Mappers.DailyCheckProfile
{
    public class DailyCheckInProfile : Profile
    {
        public DailyCheckInProfile()
        {
            CreateMap<DailyCheckIn, DailyCheckInDTO>().ReverseMap();
        }
    }
}

using AutoMapper;
using CGP.Contract.DTO.ActivityLog;
using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Infrastructure.Mappers.ActivityLogProfile
{
    public class ActivityLogProfile : Profile
    {
        public ActivityLogProfile()
        {
            CreateMap<ActivityLog, ViewActivityDTO>().ReverseMap();
        }
    }
}

using AutoMapper;
using CGP.Contract.DTO.UserQuest;
using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Infrastructure.Mappers.UserQuestProfile
{
    public class UserQuestProfile : Profile
    {
        public UserQuestProfile()
        {
            CreateMap<UserQuest, ViewUserQuestDTO>().ReverseMap();
        }
    }
}

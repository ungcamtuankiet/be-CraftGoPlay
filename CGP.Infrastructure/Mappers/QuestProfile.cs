using AutoMapper;
using CGP.Contract.DTO.Quest;
using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Infrastructure.Mappers
{
    public class QuestProfile : Profile
    {
        public QuestProfile()
        {
            CreateMap<Quest, CreateQuestDTO>().ReverseMap();
        }
    }
}

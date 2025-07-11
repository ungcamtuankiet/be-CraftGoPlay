using AutoMapper;
using CGP.Contract.DTO.CraftSkill;
using CGP.Domain.Entities;
namespace CGP.Infrastructure.Mappers.CraftSkill
{
    public class CraftSkillProfile : Profile
    {
        public CraftSkillProfile()
        {
            CreateMap<ViewCraftSkillDTO, CGP.Domain.Entities.CraftSkill>().ReverseMap();
            CreateMap<CreateCraftSkillDTO, CGP.Domain.Entities.CraftSkill>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Artisans, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<UpdateCraftSkillDTO, CGP.Domain.Entities.CraftSkill>().ReverseMap();
        }
    }
}

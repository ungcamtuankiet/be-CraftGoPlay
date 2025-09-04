using CGP.Contract.DTO.CraftSkill;
using CGP.Contract.DTO.CraftVillage;
using CGP.Contract.DTO.User;
using CGP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.ArtisanRequest
{
    public class ViewArtisanRequestDTO
    {
        public Guid Id { get; set; }
        public string Image { get; set; }
        public Guid UserId { get; set; }
        public int YearsOfExperience { get; set; }
        public string Description { get; set; }
        public string Reason { get; set; }
        public RequestArtisanStatus Status { get; set; }
        public DateTime? ModificationDate { get; set; }
    }
}

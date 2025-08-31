using CGP.Contract.DTO.CraftVillage;
using CGP.Contract.DTO.UserAddress;
using CGP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.User
{
    public class ArtisanDTO
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Thumbnail { get; set; }
        public ViewCraftVillageDTO? CraftVillage { get; set; }
    }
}

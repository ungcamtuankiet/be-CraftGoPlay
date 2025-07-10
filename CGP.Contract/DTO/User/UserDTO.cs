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
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Thumbnail { get; set; }
        public StatusEnum? Status { get; set; }
        public int RoleId { get; set; }
        public ViewCraftVillageDTO? CraftVillage { get; set; }
        public List<ViewAddressDTO> UserAddresses { get; set; } = new List<ViewAddressDTO>();
    }
}

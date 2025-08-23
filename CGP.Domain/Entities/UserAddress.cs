using CGP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Domain.Entities
{
    public class UserAddress : BaseEntity
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public TypeAddressEnum AddressType { get; set; }
        public bool IsDefault { get; set; } 
        public string FullAddress { get; set; }
        public int ProviceId { get; set; }
        public string ProviceName { get; set; }
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
        public string WardCode { get; set; }
        public string WardName { get; set; }
        public string HomeNumber { get; set; }
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

    }
}

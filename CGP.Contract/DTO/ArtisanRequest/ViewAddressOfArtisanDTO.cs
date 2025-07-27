using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.ArtisanRequest
{
    public class ViewAddressOfArtisanDTO
    {
        public Guid UserId { get; set; }
        public string PhoneNumber { get; set; }
        public int ProviceId { get; set; }
        public int DistrictId { get; set; }
        public string WardCode { get; set; }
        public string ProviceName { get; set; }
        public string DistrictName { get; set; }
        public string WardName { get; set; }
        public string HomeNumber { get; set; }
        public string FullAddress { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.UserAddress
{
    public class UpdateAddressDTO
    {
        public string FullAddress { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}

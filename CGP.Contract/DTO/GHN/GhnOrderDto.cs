using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.GHN
{
    public class GhnOrderDto
    {
        public string ToName { get; set; }
        public string ToPhone { get; set; }
        public string ToAddress { get; set; }
        public int ToDistrictId { get; set; }
        public int ToWardCode { get; set; }
        public int Weight { get; set; }
        public int ServiceTypeId { get; set; }
        public int PaymentTypeId { get; set; } // 1: Người nhận trả phí, 2: Shop trả
        public List<GhnItemDto> Items { get; set; }
    }

}

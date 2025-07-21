using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.GHN
{
    public class GhnOrderDto
    {
        public int FromDistrictId { get; set; }
        public int ToDistrictId { get; set; }
        public string FromWardCode { get; set; }
        public string ToWardCode { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public List<GhnItemDto> Items { get; set; }
    }

}

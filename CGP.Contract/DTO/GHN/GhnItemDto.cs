using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.GHN
{
    public class GhnItemDto
    {
        public int Quantity { get; set; }
        public double Weight { get; set; }
        public decimal UnitPrice { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}

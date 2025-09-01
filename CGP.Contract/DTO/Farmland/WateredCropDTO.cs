using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.Farmland
{
    public class WateredCropDTO
    {
        public Guid UserId { get; set; }
        public int TileId { get; set; }
        public DateTime NextWaterDueAtUtc { get; set; }
    }
}

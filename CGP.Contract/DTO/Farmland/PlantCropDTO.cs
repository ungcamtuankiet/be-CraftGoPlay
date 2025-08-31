using CGP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.Farmland
{
    public class PlantCropDTO
    {
        public Guid UserId { get; set; }
        public int TileId { get; set; }
        public Guid ItemId { get; set; }
        public DateTime NextWaterDueAtUtc { get; set; }
    }
}

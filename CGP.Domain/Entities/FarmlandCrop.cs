using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Domain.Entities
{
    public class FarmlandCrop : BaseEntity
    {
        public Guid FarmlandId { get; set; }
        public Guid SeedId { get; set; }
        public int Stage { get; set; }
        public int TileId { get; set; }
        public bool NeedsWater { get; set; }
        public DateTime NextWaterDueAtUtc { get; set; }
        public DateTime StageEndsAtUtc  { get; set; }
        public DateTime HarvestableAtUtc { get; set; }
        public DateTime PlantedAtUtc  { get; set; }
        public DateTime HarvestedAtUtc   { get; set; }
        public bool IsActive { get; set; }  

        public FarmLand Farmland { get; set; }
        public Item Item { get; set; }
    }
}

using CGP.Contract.DTO.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.FarmlandCrop
{
    public class ViewFarmlandCropDTO
    {
        public Guid Id { get; set; }
        public int TileId { get; set; }
        public Guid SeedId { get; set; }
        public Guid UserId { get; set; }
        public int Stage { get; set; }
        public bool NeedsWater { get; set; }
        public DateTime? NextWaterDueAtUtc { get; set; }
        public DateTime? StageEndsAtUtc { get; set; }
        public DateTime? HarvestableAtUtc { get; set; }
        public DateTime? PlantedAtUtc { get; set; }
        public DateTime? HarvestedAtUtc { get; set; }
        public bool IsActive { get; set; }
        public ViewItemDTO Item { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Domain.Entities
{
    public class Crop : BaseEntity
    {
        public Guid UserId { get; set; }
        public string CropType { get; set; } 
        public int TileX { get; set; }
        public int TileY { get; set; }
        public int GrowthStage { get; set; } = 0;
        public int WaterCount { get; set; } = 0;
        public bool IsHarvested { get; set; } = false;
        public DateTime PlantedAt { get; set; } = DateTime.UtcNow.AddHours(7);

        public ApplicationUser User { get; set; }
    }
}

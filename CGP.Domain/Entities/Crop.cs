using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Domain.Entities
{
    public class Crop : BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string CropType { get; set; } 
        public int GrowthStage { get; set; } 
        public int WaterCount { get; set; }
        public int GrowthTimeHours { get; set; }
        public int WateringIntervalHours { get; set; }
        public string? ImageUrl { get; set; }
        public ICollection<FarmlandCrop> FarmlandCrops { get; set; } = new List<FarmlandCrop>();
        public ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
    }
}

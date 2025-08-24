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
        public FarmLand Farmland { get; set; }

        public Guid CropId { get; set; }
        public Crop Crop { get; set; }

        public DateTime PlantDate { get; set; }
        public bool IsHarvested { get; set; } = false;
        public DateTime? HarvestDate { get; set; }
        public int WateredCount { get; set; } = 0;
        public DateTime? LastWateredAt { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.Crop
{
    public class ViewCropDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string CropType { get; set; }
        public int TileX { get; set; }
        public int TileY { get; set; }
        public int GrowthStage { get; set; }
        public int WaterCount { get; set; }
        public bool IsHarvested { get; set; }
        public DateTime PlantedAt { get; set; }
    }
}

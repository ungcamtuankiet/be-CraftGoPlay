using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.Crop
{
    public class PlantCropDTO
    {
        public Guid UserId { get; set; }
        public string CropType { get; set; }
        public int TileX { get; set; }
        public int TileY { get; set; }
    }
}

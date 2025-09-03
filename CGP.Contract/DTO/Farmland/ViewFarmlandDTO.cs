using CGP.Contract.DTO.FarmlandCrop;
using CGP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.Farmland
{
    public class ViewFarmlandDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int TileId { get; set; }
        public bool Watered { get; set; }
        public DateTime? WaterExpiresAt { get; set; }
        public DateTime? PlantedAt { get; set; }
        public FarmLandStatus Status { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public List<ViewFarmlandCropDTO> FarmlandCrops { get; set; } = new List<ViewFarmlandCropDTO>();
    }
}

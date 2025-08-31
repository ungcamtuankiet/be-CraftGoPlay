using CGP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Domain.Entities
{
    public class FarmLand : BaseEntity
    {
        public Guid UserId { get; set; }
        public bool Watered { get; set; }
        public DateTime WaterExpiresAt { get; set; }
        public DateTime PlantedAt { get; set; }
        public FarmLandStatus Status { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<FarmlandCrop> FarmlandCrops { get; set; } = new List<FarmlandCrop>();
    }
}

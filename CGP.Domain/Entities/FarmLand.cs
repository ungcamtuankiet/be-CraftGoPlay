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
        public int PlotIndex { get; set; }
        public bool IsDug { get; set; }
        public bool HasCrop { get; set; }
        public DateTime PlantedAt { get; set; } = DateTime.UtcNow.AddHours(7);
        public ApplicationUser User { get; set; }
    }
}

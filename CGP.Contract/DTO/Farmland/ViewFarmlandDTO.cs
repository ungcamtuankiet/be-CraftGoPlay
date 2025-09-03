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
        public int PlotIndex { get; set; }
        public bool IsDug { get; set; }
        public bool HasCrop { get; set; }
        public DateTime PlantedAt { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
    }
}

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.Crop
{
    public class UpdateCropDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string CropType { get; set; }
        public int GrowthStage { get; set; }
        public int WaterCount { get; set; }
        public int GrowthTimeHours { get; set; }
        public int WateringIntervalHours { get; set; }
        public IFormFile? ImageUrl { get; set; }
    }
}

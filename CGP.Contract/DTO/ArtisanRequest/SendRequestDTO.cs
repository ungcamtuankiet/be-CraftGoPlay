using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.ArtisanRequest
{
    public class SendRequestDTO
    {
        public IFormFile Image { get; set; }
        public Guid CraftVillageId { get; set; }
        public Guid UserId { get; set; }
        public int YearsOfExperience { get; set; }
        public string Description { get; set; }
    }
}

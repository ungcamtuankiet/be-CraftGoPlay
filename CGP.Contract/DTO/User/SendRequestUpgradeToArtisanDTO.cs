using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.User
{
    public class SendRequestUpgradeToArtisanDTO
    {
        public IFormFile Image { get; set; }
        public string CraftVillageName { get; set; }
        public int YearsOfExperience { get; set; }
        public string Description { get; set; }
    }
}

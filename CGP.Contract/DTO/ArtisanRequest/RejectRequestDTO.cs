using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.ArtisanRequest
{
    public class RejectRequestDTO
    {
        public Guid Id { get; set; }
        public string Reason { get; set; }
    }
}

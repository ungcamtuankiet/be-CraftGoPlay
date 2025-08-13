using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.DailyCheckIn
{
    public class DailyCheckInDTO
    {
        public Guid UserId { get; set; }
        public string? Note { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Domain.Entities
{
    public class DailyCheckIn : BaseEntity
    {
        public Guid UserId { get; set; }
        public DateTime CheckInDate { get; set; }
        public decimal? Reward { get; set; }
        public string? Note { get; set; }
        public int StreakCount { get; set; }

        public ApplicationUser User { get; set; }

    }
}

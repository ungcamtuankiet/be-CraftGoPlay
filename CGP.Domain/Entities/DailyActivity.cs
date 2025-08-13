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
        public DateTime CheckInDate { get; set; } = DateTime.UtcNow.AddHours(7);
        public decimal? Reward { get; set; }
        public string? Note { get; set; }
        public int StreakCount { get; set; } = 0;
        public ApplicationUser User { get; set; }

    }
}

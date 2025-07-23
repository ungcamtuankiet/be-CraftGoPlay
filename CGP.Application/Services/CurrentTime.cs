using CGP.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Services
{
    public class CurrentTime : ICurrentTime
    {
        public DateTime GetCurrentTime()
        {
            // Lấy thời gian UTC và chuyển sang UTC+7
            var utcNow = DateTime.UtcNow;
            var utcPlus7 = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"); // Múi giờ UTC+7
            return TimeZoneInfo.ConvertTimeFromUtc(utcNow, utcPlus7);
        }
    }
}

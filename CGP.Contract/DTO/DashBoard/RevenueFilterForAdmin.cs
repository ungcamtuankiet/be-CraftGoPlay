using CGP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.DashBoard
{
    public class RevenueFilterForAdmin
    {
        // Kiểu filter: "day", "week", "month", "year", hoặc "custom"
        public RevenueFilterType Type { get; set; }

        // Nếu type = "custom" thì dùng from - to
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}

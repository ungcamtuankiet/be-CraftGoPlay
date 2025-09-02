using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.DashBoard
{
    public class OrderDashboardForArtisanDto
    {
        public int TotalOrders { get; set; }

        //Dictionary lưu theo status -> số lượng
        public Dictionary<string, int> OrderStatusCounts { get; set; } = new();

        //Doanh thu
        public decimal TotalRevenue { get; set; }
        public decimal TotalRevenueBeforeFee { get; set; }
        public decimal TotalRevenueAfterFee { get; set; }
    }
}

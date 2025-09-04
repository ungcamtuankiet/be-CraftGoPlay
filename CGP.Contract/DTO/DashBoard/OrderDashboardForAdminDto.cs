using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.DashBoard
{
    public class OrderDashboardForAdminDto
    {
        public int TotalOrders { get; set; }

        //Dictionary lưu theo status -> số lượng
        public Dictionary<string, int> OrderStatusCounts { get; set; } = new();

        //Doanh thu
        public decimal TotalRevenueBeforeFee { get; set; }
        public decimal TotalRevenueDeliveryFee { get; set; }
        public decimal TotalRevenueProductFee { get; set; }
        public decimal TotalRevenueAfterFee { get; set; }
    }
}

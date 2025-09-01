using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.Order
{
    public class OrderCountDto
    {
        public int TotalOrders { get; set; }
        public int CompletedOrders { get; set; }
        public int CancelledOrders { get; set; }
        public int RefundedOrders { get; set; }
        public int RejectedOrders { get; set; }
        public int DeliveryFailedOrders { get; set; }
    }
}

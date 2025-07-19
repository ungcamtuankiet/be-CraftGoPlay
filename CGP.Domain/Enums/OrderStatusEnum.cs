using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Domain.Enums
{
    public enum OrderStatusEnum
    {
        Pending,
        Accepted,
        Rejected,
        Paid,
        Cancelled,
        Shipped,
        Refund,
        WaitingForPayment,
        Processing,
        Completed
    }
}

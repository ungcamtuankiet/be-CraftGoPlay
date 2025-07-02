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
        Paid,
        Cancelled,
        Shipped,
        WaitingForPayment
    }
}

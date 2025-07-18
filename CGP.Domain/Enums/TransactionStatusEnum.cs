using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Domain.Enums
{
    public enum TransactionStatusEnum
    {
        Pending,
        Processing,
        Success,
        Failed,
        Cancelled,
        Refunded,
        Expired,
        Verifying,
        OnHold
    }
}

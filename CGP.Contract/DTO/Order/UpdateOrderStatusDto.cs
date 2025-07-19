using CGP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.Order
{
    public class UpdateOrderStatusDto
    {
        public OrderStatusEnum Status { get; set; }
    }
}

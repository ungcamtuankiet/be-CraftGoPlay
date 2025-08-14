using CGP.Contract.DTO.Product;
using CGP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.OrderItem
{
    public class ViewOrderItemDTO
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string ArtisanName { get; set; }
        public OrderStatusEnum Status { get; set; }
        public ViewProductOrderDTO Product { get; set; }
    }
}

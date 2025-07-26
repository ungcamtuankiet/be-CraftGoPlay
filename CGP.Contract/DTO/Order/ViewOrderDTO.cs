using CGP.Contract.DTO.OrderItem;
using CGP.Contract.DTO.Product;
using CGP.Contract.DTO.User;
using CGP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.Order
{
    public class ViewOrderDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public OrderStatusEnum Status { get; set; }
        public PaymentMethodEnum PaymentMethod { get; set; }
        public double Delivery_Amount { get; set; }
        public bool IsPaid { get; set; } = false;
        public DateTime CreationDate { get; set; }
        public List<ViewOrderItemDTO> OrderItems { get; set; } = new List<ViewOrderItemDTO>();
    }
}

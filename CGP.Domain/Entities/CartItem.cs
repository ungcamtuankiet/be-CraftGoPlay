using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Domain.Entities
{
    public class CartItem : BaseEntity
    {
        public Guid CartId { get; set; }

        public Guid ProductId { get; set; } 

        public int Quantity { get; set; }
        public Guid UserId { get; set; }

        public decimal UnitPrice { get; set; } 

        public decimal TotalPrice => UnitPrice * Quantity;
        public DateTime AddedAt { get; set; } = DateTime.UtcNow.AddHours(7);
        public Cart Cart { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}

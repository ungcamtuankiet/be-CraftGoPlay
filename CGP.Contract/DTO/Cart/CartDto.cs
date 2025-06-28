using CGP.Contract.DTO.CartItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.Cart
{
    public class CartDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsCheckedOut { get; set; }

        public List<CartItemDto> Items { get; set; } = new();
        public decimal TotalPrice => Items.Sum(x => x.TotalPrice);
    }
}

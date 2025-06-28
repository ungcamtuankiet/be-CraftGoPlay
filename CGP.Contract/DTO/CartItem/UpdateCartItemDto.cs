using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.CartItem
{
    public class UpdateCartItemDto
    {
        public Guid CartItemId { get; set; }
        public int Quantity { get; set; }
    }
}

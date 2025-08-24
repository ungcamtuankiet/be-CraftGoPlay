using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Domain.Entities
{
    public class ShopPrice : BaseEntity
    {
        public Guid ItemId { get; set; }
        public int Price { get; set; }
        public Item Item { get; set; }
    }
}

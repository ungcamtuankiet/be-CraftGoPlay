using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.ShopPrice
{
    public class CreateItemShopPriceDTO
    {
        public Guid ItemId { get; set; }
        public int Price { get; set; }
    }
}

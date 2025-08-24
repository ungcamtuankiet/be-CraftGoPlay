using CGP.Contract.DTO.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.ShopPrice
{
    public class ViewItemShopPriceDTO
    {
        public Guid Id { get; set; }
        public int Price { get; set; }
        public ViewItemDTO Item { get; set; }
    }
}

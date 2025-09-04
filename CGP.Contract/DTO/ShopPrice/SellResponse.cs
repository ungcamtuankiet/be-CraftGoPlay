using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.ShopPrice
{
    public class SellResponse
    {
        public int ReceivedCoins { get; set; }
        public Guid ItemId { get; set; }
        public int QuantitySold { get; set; }
        public int UnitPrice { get; set; }
        public int ToalPrice { get; set; }
        public int NewBalance { get; set; }
    }
}

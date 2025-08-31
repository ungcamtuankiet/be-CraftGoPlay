using CGP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Domain.Entities
{
    public class Item : BaseEntity
    {
        public string NameItem { get; set; }
        public string? Description { get; set; }
        public ItemTypeEnum ItemType { get; set; }
        public bool IsStackable { get; set; }
        public ShopPrice ShopPrice { get; set; }
        public ICollection<SaleTransaction> SaleTransactions { get; set; } = new List<SaleTransaction>();
        public ICollection<Inventory> Inventories { get; set; }
        public ICollection<FarmlandCrop> FarmlandCrops { get; set; } = new List<FarmlandCrop>();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.Inventory
{
    public class AddItemToInventoryDTO
    {
        public Guid UserId { get; set; }
        public Guid? ItemId { get; set; }
        public int Quantity { get; set; }
        public string ItemType { get; set; }
        public string InventoryType { get; set; }
        public int SlotIndex { get; set; }
    }
}

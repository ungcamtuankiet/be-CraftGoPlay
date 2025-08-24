using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.Inventory
{
    public class AddToInventoryDTO
    {
        public Guid UserId { get; set; }
        public Guid? CropId { get; set; } 
        public int Quantity { get; set; }
        public string InventoryType { get; set; }
        public int SlotIndex { get; set; }
    }
}

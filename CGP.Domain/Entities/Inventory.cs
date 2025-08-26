using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Domain.Entities
{
    public class Inventory : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid? CropId { get; set; }
        public Guid? ItemId { get; set; }
        public string ItemType { get; set; }
        public int Quantity { get; set; } = 0;
        public string InventoryType { get; set; } 
        public int SlotIndex { get; set; }
        public DateTime AcquiredAt { get; set; } = DateTime.UtcNow.AddHours(7);
        public ApplicationUser User { get; set; }
    }
}

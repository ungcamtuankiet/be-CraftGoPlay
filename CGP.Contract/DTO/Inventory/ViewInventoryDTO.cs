
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.Inventory
{
    public class ViewInventoryDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ItemId { get; set; }
        public string ItemType { get; set; }
        public int Quantity { get; set; }
        public string InventoryType { get; set; }
        public int SlotIndex { get; set; }
        public DateTime AcquiredAt { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }
    }
}

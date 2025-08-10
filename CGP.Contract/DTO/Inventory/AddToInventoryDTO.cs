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
        public string ItemType { get; set; }
        public int Quantity { get; set; }
    }
}

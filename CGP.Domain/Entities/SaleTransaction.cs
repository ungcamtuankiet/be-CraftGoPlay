using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Domain.Entities
{
    public class SaleTransaction : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid ItemId { get; set; }  
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }  
        public int TotalPrice { get; set; } 
        public DateTime SoldAt { get; set; }
        public ApplicationUser User { get; set; }
        public Item Item { get; set; }
    }
}

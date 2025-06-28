using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Domain.Entities
{
    public class Cart : BaseEntity
    {
        public Guid UserId { get; set; }
        public bool IsCheckedOut { get; set; } = false;
        public ApplicationUser User { get; set; }
        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
    }
}

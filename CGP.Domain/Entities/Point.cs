using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Domain.Entities
{
    public class Point : BaseEntity
    {
        public Guid UserId { get; set; }
        public int Amount { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(7);
        public DateTime? UpdatedAt { get; set; }
        public ApplicationUser User { get; set; } 
    }
}

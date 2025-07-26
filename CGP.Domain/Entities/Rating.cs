using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Domain.Entities
{
    public class Rating : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }

        [Range(1, 5)]
        public int Star { get; set; }

        [MaxLength(1000)]
        public string? Comment { get; set; }

        public DateTime RatedAt { get; set; } = DateTime.UtcNow.AddHours(7);

        public virtual ApplicationUser User { get; set; }
        public virtual Product Product { get; set; }
    }
}

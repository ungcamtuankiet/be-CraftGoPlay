using CGP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Domain.Entities
{
    public class SubCategory : BaseEntity
    {
        public string Image { get; set; }
        public string SubName { get; set; }
        public CategoryStatusEnum Status { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Domain.Entities
{
    public class SubCategory : BaseEntity
    {
        public string SubName { get; set; }
        public int Status { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }

    }
}

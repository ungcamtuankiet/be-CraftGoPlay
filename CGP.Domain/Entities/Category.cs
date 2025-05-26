using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Domain.Entities
{
    public class Category : BaseEntity
    {
        public Category()
        {
            SubCategories = new HashSet<SubCategory>();
        }
        public string CategoryName { get; set; }
        public int CategoryStatus { get; set; }
        public virtual ICollection<SubCategory> SubCategories { get; set; }
    }
}

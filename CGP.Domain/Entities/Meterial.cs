using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Domain.Entities
{
    public class Meterial : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<ProductMeterial> ProductMeterials { get; set; } = new List<ProductMeterial>();
    }
}

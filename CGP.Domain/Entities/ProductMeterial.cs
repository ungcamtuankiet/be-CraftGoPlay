using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Domain.Entities
{
    public class ProductMeterial
    {
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public Guid MeterialId { get; set; }
        public Meterial Meterial { get; set; }
    }
}

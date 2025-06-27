using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Domain.Entities
{
    public class ProductImage : BaseEntity
    {
        public string ImageUrl { get; set; }
        public Guid ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}

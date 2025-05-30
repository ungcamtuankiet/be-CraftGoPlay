using CGP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public ProductStatusEnum Status { get; set; }

        //Relationships
        public Guid Artisan_id { get; set; }
        [ForeignKey("Artisan_id")]
        public ApplicationUser User { get; set; }
        public Guid SubCategoryId { get; set; }
        [ForeignKey("SubCategoryId")]
        public SubCategory SubCategory { get; set; }
        public ICollection<ProductMeterial> ProductMeterials { get; set; } = new List<ProductMeterial>();
    }
}

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
        public decimal Price { get; set; }
        public ProductStatusEnum Status { get; set; }
        public int Quantity { get; set; }
        public int QuantitySold { get; set; }
        //Relationships
        public Guid Artisan_id { get; set; }
        [ForeignKey("Artisan_id")]
        public ApplicationUser User { get; set; }
        public Guid SubCategoryId { get; set; }
        [ForeignKey("SubCategoryId")]
        public SubCategory SubCategory { get; set; }
        public ICollection<Meterial> Meterials { get; set; } = new List<Meterial>();
        public ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public ICollection<Favourite> Favourites { get; set; } = new List<Favourite>();
    }
}

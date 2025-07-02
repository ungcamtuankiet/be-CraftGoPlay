using CGP.Contract.DTO.ProductImage;
using CGP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.Product
{
    public class ViewProductFavouriteDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public ProductStatusEnum Status { get; set; }
        public int Quantity { get; set; }
        public int QuantitySold { get; set; }
        public List<ProductImageDTO> ProductImages { get; set; } = new();
    }
}

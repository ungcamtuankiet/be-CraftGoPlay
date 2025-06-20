using CGP.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.Product
{
    public class ProductUpdateDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
        public decimal Price { get; set; }
        public ProductStatusEnum Status { get; set; }
        public Guid SubCategoryId { get; set; }
        public List<Guid> MeterialIds { get; set; } = new();
    }
}

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
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
        public decimal Price { get; set; }
        public ProductStatusEnum Status { get; set; }
        public Guid SubCategoryId { get; set; }
        public Guid Artisan_id { get; set; }
        public List<Guid> MeterialIdsToAdd { get; set; } = new();   
        public List<Guid> MeterialIdsToRemove { get; set; } = new(); 
    }
}

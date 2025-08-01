﻿using CGP.Contract.DTO.Meterial;
using CGP.Contract.DTO.ProductImage;
using CGP.Contract.DTO.Rating;
using CGP.Domain.Entities;
using CGP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.Product
{
    public class ViewProductDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public ProductStatusEnum Status { get; set; }
        public int Quantity { get; set; }
        public int QuantitySold { get; set; }
        public Guid Artisan_id { get; set; }
        public Guid SubCategoryId { get; set; }
        public string? ArtisanName { get; set; }
        public string? SubCategoryName { get; set; }
        public int Weight { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public List<MeterialDto> Meterials { get; set; } = new();
        public List<ProductImageDTO> ProductImages { get; set; } = new();
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public List<ViewRatingProductDTO> Ratings { get; set; } = new();
    }
}

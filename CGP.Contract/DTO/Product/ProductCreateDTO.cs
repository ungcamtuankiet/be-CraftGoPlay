using CGP.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.Product
{
    public class ProductCreateDto
    {
        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc.")]
        [MaxLength(50, ErrorMessage = "Tên sản phẩm phải ít hơn 50 ký tự.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Mô tả là bắt buộc.")]
        [MaxLength(255, ErrorMessage = "Mô tả phải ít hơn 255 ký tự.")]
        public string Description { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Giá phải lớn hơn hoặc bằng 0.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Trạng thái sản phẩm là bắt buộc.")]
        public ProductStatusEnum Status { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn hoặc bằng 0.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Nghệ nhân là bắt buộc.")]
        public Guid Artisan_id { get; set; }

        [Required(ErrorMessage = "Danh mục con là bắt buộc.")]
        public Guid SubCategoryId { get; set; }

        [Required(ErrorMessage = "Cân nặng sản phẩm là bắt buộc.")]
        [Range(0, 1600000, ErrorMessage = "Cân nặng phải ít hơn 1.600.000 (gram).")]
        public int Weight { get; set; }

        [Required(ErrorMessage = "Chiều dài sản phẩm là bắt buộc.")]
        [Range(0, 200, ErrorMessage = "Chiều dài tối đa là 200 (cm).")]
        public int Length { get; set; }

        [Required(ErrorMessage = "Chiều rộng sản phẩm là bắt buộc.")]
        [Range(0, 200, ErrorMessage = "Chiều rộng tối đa là 200 (cm).")]
        public int Width { get; set; }

        [Required(ErrorMessage = "Chiều cao sản phẩm là bắt buộc.")]
        [Range(0, 200, ErrorMessage = "Chiều cao tối đa là 200 (cm).")]
        public int Height { get; set; }
        public List<IFormFile> Images { get; set; } 
        public List<Guid> MeterialIds { get; set; } = new();
    }
}

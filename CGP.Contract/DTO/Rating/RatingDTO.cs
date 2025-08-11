using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.Rating
{
    public class RatingDTO
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public Guid OrderItemId { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Số sao đánh giá phải nằm trong khoảng từ 1 đến 5.")]
        public int Star { get; set; }

        [MaxLength(1000, ErrorMessage = "Bình luận không được vượt quá 1000 ký tự.")]
        public string? Comment { get; set; }

    }
}

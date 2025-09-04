using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.ShopPrice
{
    public class SellRequest
    {
        [Required(ErrorMessage = "Mã người dùng là bắt buộc")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Mã sản phẩm là bắt buộc")]
        public Guid ItemId { get; set; }
        [Required(ErrorMessage = "Số lượng là bắt buộc")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn hoặc bằng 0")]
        public int Quantity { get; set; }
    }
}

using CGP.Contract.DTO.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.Rating
{
    public class ViewRatingDTO
    {
        public string UserName { get; set; }
        public string Thumbnail { get; set; } 
        public int Star { get; set; }
        public string? Comment { get; set; }
        public DateTime RatedAt { get; set; }
        public ProductDTO Product { get; set; }
    }
}

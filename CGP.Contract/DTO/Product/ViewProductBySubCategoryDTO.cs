using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.Product
{
    public class ViewProductBySubCategoryDTO
    {
        public Guid SubId { get; set; }
        public string SubName { get; set; }
        public List<ViewProductDTO> Products { get; set; } = new List<ViewProductDTO>();

    }
}

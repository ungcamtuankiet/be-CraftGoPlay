using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.Category
{
    public class UpdateCategoryDTO
    {
        public required string CategoryName { get; set; }
        public int CategoryStatus { get; set; }
    }
}

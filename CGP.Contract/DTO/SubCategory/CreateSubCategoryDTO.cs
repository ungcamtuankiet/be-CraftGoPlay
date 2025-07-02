using CGP.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.SubCategory
{
    public class CreateSubCategoryDTO
    {
        public required string SubName { get; set; }
        public required IFormFile Image { get; set; }
        public required CategoryStatusEnum Status { get; set; }
    }
}

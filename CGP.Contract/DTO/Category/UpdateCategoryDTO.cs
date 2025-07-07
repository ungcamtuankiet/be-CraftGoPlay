using CGP.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.Category
{
    public class UpdateCategoryDTO
    {
        public string CategoryName { get; set; }
        public IFormFile Image { get; set; }

        public CategoryStatusEnum CategoryStatus { get; set; }
    }
}

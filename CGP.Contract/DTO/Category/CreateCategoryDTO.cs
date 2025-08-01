﻿using CGP.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.Category
{
    public class CreateCategoryDTO
    {
        public required string CategoryName { get; set; }
        public required IFormFile Image { get; set; }
        public required CategoryStatusEnum CategoryStatus { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.DashBoard
{
    public class ProductCountByMonthDto
    {
        public List<int> AvailableProducts { get; set; }
        public List<int> SoldProducts { get; set; }
        public int Year { get; set; }
    }
}

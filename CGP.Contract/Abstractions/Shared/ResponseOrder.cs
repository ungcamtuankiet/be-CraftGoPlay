using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.Abstractions.Shared
{
    public class ResponseOrder<T>
    {
        public int? Error { get; set; }
        public string? Message { get; set; }
        public int? Count { get; set; }
        public double? TotalPrice { get; set; }
        public T? Data { get; set; }
    }
}

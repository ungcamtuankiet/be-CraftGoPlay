using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.Abstractions.CloudinaryService
{
    public class CloudinaryResponse
    {
        public string? FileUrl { get; set; }
        public string? PublicFileId { get; set; }
    }
}

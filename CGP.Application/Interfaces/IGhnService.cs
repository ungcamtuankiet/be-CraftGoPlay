using CGP.Contract.DTO.GHN;
using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Interfaces
{
    public interface IGhnService
    {
        public Task<double> CalculateShippingFeeAsync(Guid orderId, Guid artisanId, List<GhnItemDto> itmes);
    }
}

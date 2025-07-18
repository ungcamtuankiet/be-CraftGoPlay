using CGP.Contract.DTO.GHN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Interfaces
{
    public interface IGhnService
    {
        Task<int> CalculateShippingFeeAsync(int fromDistrictId, int toDistrictId, int serviceTypeId, int weight);
        Task<string> CreateShippingOrderAsync(GhnOrderDto orderDto);
    }
}

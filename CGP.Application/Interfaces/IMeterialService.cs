using CGP.Contract.DTO.Meterial;
using CGP.Contracts.Abstractions.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Interfaces
{
    public interface IMeterialService
    {
        public Task<Result<List<ViewMeterialDTO>>> GetMeterialsAsync();
        public Task<Result<object>> CreateMeterial(MeterialCreateDTO request);
    }
}

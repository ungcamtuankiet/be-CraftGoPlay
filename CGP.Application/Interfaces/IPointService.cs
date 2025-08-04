using CGP.Contract.DTO.Point;
using CGP.Contracts.Abstractions.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Interfaces
{
    public interface IPointService
    {
        Task<Result<ViewPointDTO>> GetPointsByUserId(Guid userId);
    }
}

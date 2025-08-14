using CGP.Contract.DTO.DailyCheckIn;
using CGP.Contracts.Abstractions.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Interfaces
{
    public interface IDailyCheckInService
    {
        Task<Result<object>> CheckInAsync(DailyCheckInDTO dto);
        Task<Result<bool>> HasCheckedInTodayAsync(Guid userId);
        Task<Result<bool>> UpdateStreakAsync(Guid userId);
        Task<Result<int>> GetCurrentStreakAsync(Guid userId);
    }
}

using AutoMapper;
using CGP.Application.Interfaces;
using CGP.Contract.DTO.DailyCheckIn;
using CGP.Contracts.Abstractions.Shared;
using CGP.Domain.Entities;
using CGP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Services
{
    public class DailyCheckInService : IDailyCheckInService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DailyCheckInService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<object>> CheckInAsync(DailyCheckInDTO dto)
        {
            var result = _mapper.Map<DailyCheckIn>(dto);
            var pointTransaction = new PointTransaction();
            var checkUser = await _unitOfWork.userRepository.GetByIdAsync(dto.UserId);

            if (checkUser == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Người dùng không tồn tại."
                };
            }

            int reward = 100;
            var checkCheckIn = await _unitOfWork.dailyCheckInRepository.IsCheckIn(dto.UserId);
            var getUserPoint = await _unitOfWork.pointRepository.GetPointsByUserId(dto.UserId);
            if (checkCheckIn == null)
            {
                getUserPoint.Amount += reward;

                getUserPoint.UpdatedAt = DateTime.UtcNow.AddHours(7);
                _unitOfWork.pointRepository.Update(getUserPoint);

                pointTransaction.Point_Id = getUserPoint.Id;
                pointTransaction.Amount = 100;
                pointTransaction.Status = PointTransactionEnum.Bonus;
                pointTransaction.Description = $"Bạn nhận được {reward} xu từ việc điểm danh hằng ngày.";
                result.StreakCount = 1;
                await _unitOfWork.dailyCheckInRepository.AddAsync(result);
                await _unitOfWork.pointTransactionRepository.AddAsync(pointTransaction);

                await _unitOfWork.SaveChangeAsync();
                return new Result<object>
                {
                    Error = 0,
                    Message = "Điểm danh thành công.",
                    Data = result
                };
            }

/*            if (checkCheckIn.CheckInDate.Date == DateTime.UtcNow.AddHours(7).Date)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Bạn đã Điểm danh hôm nay."
                };
            }*/

            if(checkCheckIn.StreakCount == 4 || checkCheckIn.StreakCount == 7)
            {
                reward = 200;
            }

            getUserPoint.Amount += reward;
            getUserPoint.UpdatedAt = DateTime.UtcNow.AddHours(7);
            _unitOfWork.pointRepository.Update(getUserPoint);

            pointTransaction.Point_Id = getUserPoint.Id;
            pointTransaction.Amount = 100;
            pointTransaction.Status = PointTransactionEnum.Bonus;
            pointTransaction.Description = $"Bạn nhận được {reward} xu từ việc điểm danh hằng ngày.";

            result.StreakCount++;
            result.Id = checkCheckIn.Id;

            await _unitOfWork.pointTransactionRepository.AddAsync(pointTransaction);
            _unitOfWork.dailyCheckInRepository.Update(result);
            await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = 0,
                Message = "Điểm danh thành công.",
                Data = result
            };
        }
    }
}

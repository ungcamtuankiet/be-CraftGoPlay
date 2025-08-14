using AutoMapper;
using CGP.Application.Interfaces;
using CGP.Contract.DTO.DailyCheckIn;
using CGP.Contracts.Abstractions.Shared;
using CGP.Domain.Entities;
using CGP.Domain.Enums;
using System;
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

        public async Task<Result<bool>> HasCheckedInTodayAsync(Guid userId)
        {
            var checkUser = await _unitOfWork.userRepository.GetByIdAsync(userId);
            if (checkUser == null)
            {
                return new Result<bool>
                {
                    Error = 1,
                    Message = "Người dùng không tồn tại.",
                    Data = false
                };
            }

            bool hasCheckedInToday = await _unitOfWork.dailyCheckInRepository.HasCheckedInToday(userId);
            return new Result<bool>
            {
                Error = 0,
                Message = hasCheckedInToday ? "Người dùng đã điểm danh hôm nay." : "Người dùng chưa điểm danh hôm nay.",
                Data = hasCheckedInToday
            };
        }

        public async Task<Result<int>> GetCurrentStreakAsync(Guid userId)
        {
            var checkUser = await _unitOfWork.userRepository.GetByIdAsync(userId);
            if (checkUser == null)
            {
                return new Result<int>
                {
                    Error = 1,
                    Message = "Người dùng không tồn tại.",
                    Data = 0
                };
            }

            var checkIn = await _unitOfWork.dailyCheckInRepository.IsCheckIn(userId);
            if (checkIn == null)
            {
                return new Result<int>
                {
                    Error = 0,
                    Message = "Người dùng chưa có lịch sử điểm danh.",
                    Data = 0
                };
            }

            var currentDate = DateTime.UtcNow.AddHours(7).Date;
            var yesterday = currentDate.AddDays(-1);

            // Nếu ngày điểm danh gần nhất không phải hôm qua, reset streak
            if (checkIn.CheckInDate.Date < yesterday)
            {
                checkIn.StreakCount = 0;
                _unitOfWork.dailyCheckInRepository.Update(checkIn);
                await _unitOfWork.SaveChangeAsync();
            }

            return new Result<int>
            {
                Error = 0,
                Message = "Lấy streak thành công.",
                Data = checkIn.StreakCount
            };
        }

        public async Task<Result<object>> CheckInAsync(DailyCheckInDTO dto)
        {
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
            var currentDate = DateTime.UtcNow.AddHours(7).Date;

            if (checkCheckIn == null)
            {
                var newCheckIn = _mapper.Map<DailyCheckIn>(dto);
                newCheckIn.StreakCount = 1;
                newCheckIn.CheckInDate = DateTime.UtcNow.AddHours(7);
                getUserPoint.Amount += reward;
                getUserPoint.UpdatedAt = DateTime.UtcNow.AddHours(7);
                _unitOfWork.pointRepository.Update(getUserPoint);

                pointTransaction.Point_Id = getUserPoint.Id;
                pointTransaction.Amount = reward;
                pointTransaction.Status = PointTransactionEnum.Bonus;
                pointTransaction.Description = $"Bạn nhận được {reward} xu từ việc điểm danh hằng ngày.";

                await _unitOfWork.dailyCheckInRepository.AddAsync(newCheckIn);
                await _unitOfWork.pointTransactionRepository.AddAsync(pointTransaction);
                await _unitOfWork.SaveChangeAsync();

                return new Result<object>
                {
                    Error = 0,
                    Message = "Điểm danh thành công.",
                    Data = newCheckIn
                };
            }

            // Kiểm tra xem đã điểm danh hôm nay chưa
            if (checkCheckIn.CheckInDate.Date == currentDate)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Bạn đã điểm danh hôm nay."
                };
            }

            // Kiểm tra xem có phải tiếp tục streak hay reset
            bool isConsecutiveDay = checkCheckIn.CheckInDate.Date == currentDate.AddDays(-1);
            if (!isConsecutiveDay)
            {
                checkCheckIn.StreakCount = 0; // Reset streak nếu không điểm danh liên tục
            }

            // Reset streak về 1 nếu streak hiện tại là 7
            if (checkCheckIn.StreakCount >= 7)
            {
                checkCheckIn.StreakCount = 0;
            }

            // Tính thưởng dựa trên streak
            if (checkCheckIn.StreakCount == 3 || checkCheckIn.StreakCount == 6)
            {
                reward = 200;
            }

            getUserPoint.Amount += reward;
            getUserPoint.UpdatedAt = DateTime.UtcNow.AddHours(7);
            _unitOfWork.pointRepository.Update(getUserPoint);

            pointTransaction.Point_Id = getUserPoint.Id;
            pointTransaction.Amount = reward;
            pointTransaction.Status = PointTransactionEnum.Bonus;
            pointTransaction.Description = $"Bạn nhận được {reward} xu từ việc điểm danh hằng ngày.";

            // Cập nhật checkCheckIn
            checkCheckIn.StreakCount++;
            checkCheckIn.CheckInDate = DateTime.UtcNow.AddHours(7);
            _unitOfWork.dailyCheckInRepository.Update(checkCheckIn);

            await _unitOfWork.pointTransactionRepository.AddAsync(pointTransaction);
            await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = 0,
                Message = "Điểm danh thành công.",
                Data = checkCheckIn
            };
        }

        public async Task<Result<bool>> UpdateStreakAsync(Guid userId)
        {
            var checkUser = await _unitOfWork.userRepository.GetByIdAsync(userId);
            if (checkUser == null)
            {
                return new Result<bool>
                {
                    Error = 1,
                    Message = "Người dùng không tồn tại.",
                    Data = false
                };
            }

            var checkIn = await _unitOfWork.dailyCheckInRepository.IsCheckIn(userId);
            if (checkIn == null)
            {
                return new Result<bool>
                {
                    Error = 0,
                    Message = "Người dùng chưa có lịch sử điểm danh.",
                    Data = false
                };
            }

            var currentDate = DateTime.UtcNow.AddHours(7).Date;
            var yesterday = currentDate.AddDays(-1);

            // Nếu ngày điểm danh gần nhất không phải hôm qua, reset streak
            if (checkIn.CheckInDate.Date < yesterday)
            {
                checkIn.StreakCount = 0;
                _unitOfWork.dailyCheckInRepository.Update(checkIn);
                await _unitOfWork.SaveChangeAsync();
                return new Result<bool>
                {
                    Error = 0,
                    Message = "Streak đã được reset do bỏ lỡ ngày điểm danh.",
                    Data = true
                };
            }

            return new Result<bool>
            {
                Error = 0,
                Message = "Streak không cần reset.",
                Data = false
            };
        }
    }
}
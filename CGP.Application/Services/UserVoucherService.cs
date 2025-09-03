using AutoMapper;
using CGP.Application.Interfaces;
using CGP.Contract.DTO.UserVoucher;
using CGP.Contracts.Abstractions.Shared;
using CGP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Services
{
    public class UserVoucherService : IUserVoucherService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserVoucherService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<object>> SwapVoucher(Guid userId, Guid voucherId)
        {
            var getVoucher = await _unitOfWork.voucherRepository.GetByIdAsync(voucherId);
            var getUser = await _unitOfWork.userRepository.GetByIdAsync(userId);
            var userPoint = await _unitOfWork.pointRepository.GetPointsByUserId(userId);
            var hasUserVoucher = await _unitOfWork.userRepository.CheckExistUserVoucher(userId, voucherId);
            if (getVoucher == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Mã giảm giá không tồn tại.",
                    Data = null
                };
            }

            if(hasUserVoucher)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Người dùng đã sở hữu mã giảm giá này.",
                    Data = null
                };
            }

            if (getUser == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Người dùng không tồn tại.",
                    Data = null
                };
            }

            if (getVoucher.PointChangeAmount > userPoint.Amount)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Người dùng không đủ điểm để đổi mã giảm giá.",
                    Data = null
                };
            }

            if (getVoucher.ChangeAmout == getVoucher.Quantity)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Mã giảm giá đã hết.",
                    Data = null
                };
            }

            getUser.Vouchers.Add(getVoucher);
            userPoint.Amount -= getVoucher.PointChangeAmount;
            var pointTransaction = new Domain.Entities.PointTransaction
            {
                Point_Id = userPoint.Id,
                Amount = getVoucher.PointChangeAmount,  
                Status = PointTransactionEnum.Swap,
                Description = $"Đổi mã giảm giá {getVoucher.Code}",
                CreatedAt = DateTime.UtcNow.AddHours(7)
            };
            await _unitOfWork.pointTransactionRepository.AddAsync(pointTransaction);
            getVoucher.ChangeAmout += 1;
            _unitOfWork.pointRepository.Update(userPoint);
            _unitOfWork.voucherRepository.Update(getVoucher);

            await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = 0,
                Message = "Đổi mã giảm giá thành công.",
                Data = null
            };


        }

        public async Task<Result<object>> GetAllVouchersByUserId(Guid userId, VoucherTypeEnum voucherType)
        {
            var result = _mapper.Map<List<ViewUserVoucherDTO>>(await _unitOfWork.userRepository.GetAllsVoucherByUserId(userId, voucherType));
            return new Result<object>
            {
                Error = 0,
                Message = "Lấy mã giảm giá thành công.",
                Data = result
            };
        }
    }
}

using AutoMapper;
using CGP.Application.Interfaces;
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
            if (getVoucher == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Mã giảm giá không tồn tại.",
                    Data = null
                };
            }

            if(getUser == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Người dùng không tồn tại.",
                    Data = null
                };
            }
            
            if(getVoucher.PointChangeAmount > userPoint.Amount)
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

            return new Result<object>
            {
                Error = 0,
                Message = "Đổi mã giảm giá thành công.",
                Data = getVoucher
            };


        }

        public Task<Result<object>> GetAllVouchersByUserId(Guid userId, VoucherTypeEnum voucherType)
        {
            throw new NotImplementedException();
        }
    }
}

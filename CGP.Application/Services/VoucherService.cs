using AutoMapper;
using CGP.Application.Interfaces;
using CGP.Contract.DTO.Voucher;
using CGP.Contracts.Abstractions.Shared;
using CGP.Domain.Entities;
using CGP.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Services
{
    public class VoucherService : IVoucherService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public VoucherService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        private Result<Guid> GetUserIdFromToken()
        {
            try
            {
                var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (string.IsNullOrEmpty(token))
                    return new Result<Guid>() { Error = 1, Message = "Token not found", Data = Guid.Empty };

                var jwtToken = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;

                if (jwtToken == null)
                    return new Result<Guid>() { Error = 1, Message = "Invalid token", Data = Guid.Empty };

                var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "id");
                if (userIdClaim == null)
                    return new Result<Guid>() { Error = 1, Message = "User ID not found in token", Data = Guid.Empty };

                var userId = Guid.Parse(userIdClaim.Value);
                return new Result<Guid>() { Error = 0, Message = "Success", Data = userId };
            }
            catch (Exception ex)
            {
                return new Result<Guid>() { Error = 1, Message = $"Error getting user ID: {ex.Message}", Data = Guid.Empty };
            }
        }

        public async Task<Result<List<ViewVoucherDTO>>> GetAllVouchersAsync()
        {
            // Lấy userId từ token
            var userIdResult = GetUserIdFromToken();
            if (userIdResult.Error == 1)
            {
                return new Result<List<ViewVoucherDTO>>()
                {
                    Error = 1,
                    Message = userIdResult.Message,
                    Count = 0,
                    Data = null
                };
            }

            var userId = userIdResult.Data;

            var result = _mapper.Map<List<ViewVoucherDTO>>(
                await _unitOfWork.voucherRepository.GetAvailableVouchersForUserAsync(userId));
            
            return new Result<List<ViewVoucherDTO>>()
            {
                Error = 0,
                Message = "Lấy danh sách phiếu giảm giá khả dụng thành công",
                Count = result.Count,
                Data = result
            };
        }

        public async Task<Result<ViewVoucherDTO>> GetVoucherByIdAsync(Guid voucherId)
        {
            var result = _mapper.Map<ViewVoucherDTO>(await _unitOfWork.voucherRepository.GetByIdAsync(voucherId));
            if (result == null)
            {
                return new Result<ViewVoucherDTO>()
                {
                    Error = 1,
                    Message = "Phiếu giảm giá không tồn tại",
                    Count = 0,
                    Data = null
                };
            }

            return new Result<ViewVoucherDTO>()
            {
                Error = 0,
                Message = "Lấy thông tin phiếu giảm giá thành công",
                Count = 1,
                Data = result
            };
        }

        public async Task<Result<ViewVoucherDTO>> GetAllVouchersByCodeAsync(string voucherCode)
        {
            var result = _mapper.Map<ViewVoucherDTO>(await _unitOfWork.voucherRepository.GetVoucherByCodeAsync(voucherCode));
            if(result == null)
            {
                return new Result<ViewVoucherDTO>()
                {
                    Error = 1,
                    Message = "Mã giảm giá không tồn tại",
                    Count = 0,
                    Data = null
                };
            }

            return new Result<ViewVoucherDTO>()
            {
                Error = 0,
                Message = "Lấy thông tin mã giảm giá thành công",
                Count = 1,
                Data = result
            };
        }

        public async Task<Result<object>> CreateVoucherAsync(CreateVoucherDTO dto)
        {
            var checkCode = await _unitOfWork.voucherRepository.CheckVoucherCode(dto.Code);
            if (checkCode)
            {
                return new Result<object>()
                {
                    Error = 1,
                    Message = "Mã giảm giá đã tồn tại",
                    Data = null
                };
            }

            if(dto.StartDate >= dto.EndDate)
            {
                return new Result<object>()
                {
                    Error = 1,
                    Message = "Ngày bắt đầu phải trước ngày kết thúc",
                    Data = null
                };
            }

            if(dto.DiscountType == VoucherDiscountTypeEnum.Percentage)
            {
                if (dto.Discount < 0 || dto.Discount > 100)
                {
                    return new Result<object>()
                    {
                        Error = 1,
                        Message = "Giảm giá phải từ 0 đến 100%",
                        Data = null
                    };
                }
            }

            else if (dto.DiscountType == VoucherDiscountTypeEnum.FixedAmount)
            {
                if (dto.Discount < 0)
                {
                    return new Result<object>()
                    {
                        Error = 1,
                        Message = "Giảm giá phải lớn hơn hoặc bằng 0",
                        Data = null
                    };
                }
            }
            var result = _mapper.Map<Voucher>(dto);
            await _unitOfWork.voucherRepository.AddAsync(result);
            await _unitOfWork.SaveChangeAsync();
            return new Result<object>()
            {
                Error = 0,
                Message = "Tạo phiếu giảm giá thành công",
                Data = null
            };
        }

        public async Task<Result<object>> UpdateVoucherAsync(UpdateVoucherDTO dto)
        {
            var voucher = await _unitOfWork.voucherRepository.GetByIdAsync(dto.Id);
            if (dto.Code == null)
            {
                voucher.Code = voucher.Code;
            }

            var checkCode = await _unitOfWork.voucherRepository.CheckVoucherCode(dto.Code);
            if (!checkCode)
            {
                return new Result<object>()
                {
                    Error = 1,
                    Message = "Mã giảm giá đã tồn tại",
                    Data = null
                };
            }

            if (dto.DiscountType == VoucherDiscountTypeEnum.Percentage)
            {
                if (dto.Discount < 0 || dto.Discount > 100)
                {
                    return new Result<object>()
                    {
                        Error = 1,
                        Message = "Giảm giá phải từ 0 đến 100%",
                        Data = null
                    };
                }
            }

            else if (dto.DiscountType == VoucherDiscountTypeEnum.FixedAmount)
            {
                if (dto.Discount < 0)
                {
                    return new Result<object>()
                    {
                        Error = 1,
                        Message = "Giảm giá phải lớn hơn hoặc bằng 0",
                        Data = null
                    };
                }
            }

            if (dto.StartDate >= dto.EndDate)
            {
                return new Result<object>()
                {
                    Error = 1,
                    Message = "Ngày bắt đầu phải trước ngày kết thúc",
                    Data = null
                };
            }

            var result = _mapper.Map(dto, voucher);
            _unitOfWork.voucherRepository.Update(result);
            await _unitOfWork.SaveChangeAsync();
            return new Result<object>()
            {
                Error = 0,
                Message = "Cập nhật phiếu giảm giá thành công",
                Data = null
            };
        }

        public Task<Result<object>> RemoveVoucherAsync(Guid voucherId)
        {
            throw new NotImplementedException();
        }
    }
}

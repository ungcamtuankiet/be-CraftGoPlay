using AutoMapper;
using CGP.Application.Interfaces;
using CGP.Contract.DTO.RefundRequest;
using CGP.Contracts.Abstractions.Shared;
using CGP.Domain.Entities;
using CGP.Domain.Enums;
using CloudinaryDotNet.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Services
{
    public class ReturnRequestService : IReturnRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPayoutService _payoutService;
        private readonly ICloudinaryService _cloudinaryService;
        private static string FOLDER = "return-request";

        public ReturnRequestService(IUnitOfWork unitOfWork, IMapper mapper, IPayoutService payoutService, ICloudinaryService cloudinaryService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _payoutService = payoutService;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<Result<bool>> RefundOrderAsync(SendRefundRequestDTO dto)
        {
            var order = await _unitOfWork.orderRepository.GetOrderByIdAsync(dto.OrderId);
            var returnRequest = _mapper.Map<ReturnRequest>(dto);
            var uploadResult = await _cloudinaryService.UploadProductImage(dto.ImageUrl, FOLDER);

            if (order.Status == OrderStatusEnum.Completed)
            {
                return new Result<bool>
                {
                    Error = 1,
                    Message = "Đơn hàng đã được xác nhận là nhận đơn nên không thể hoàn hàng.",
                    Data = false
                };
            }

            if (order.Status != OrderStatusEnum.Delivered)
            {
                return new Result<bool>
                {
                    Error = 1,
                    Message = "Đơn hàng chưa được giao đến tay bạn nên không thể hoàn hàng.",
                    Data = false
                };
            }

            if (order == null || order.Payment == null)
            {
                return new Result<bool>
                {
                    Error = 1,
                    Message = "Đơn hàng không tồn tại hoặc chưa thanh toán.",
                    Data = false
                };
            }

            if (order.Payment.IsRefunded)
            {
                return new Result<bool>
                {
                    Error = 1,
                    Message = "Đơn hàng đã được hoàn tiền trước đó.",
                    Data = false
                };
            }
            if (uploadResult == null || string.IsNullOrEmpty(uploadResult.SecureUrl.ToString()))
            {
                return new Result<bool>
                {
                    Error = 1,
                    Message = "Tải lên hình ảnh hoàn hàng thất bại.",
                    Data = false
                };
            }

            if (dto.ImageUrl == null)
            {
                return new Result<bool>
                {
                    Error = 1,
                    Message = "Vui lòng cung cấp hình ảnh sản phẩm để hoàn hàng.",
                    Data = false
                };
            }

            returnRequest.ImageUrl = uploadResult.SecureUrl.ToString();
            await _unitOfWork.returnRequestRepository.AddAsync(returnRequest);

            order.Status = OrderStatusEnum.ReturnRequested;


            await _unitOfWork.SaveChangeAsync();

            return new Result<bool>
            {
                Error = 0,
                Message = "Đã gửi yêu cầu hoàn trả hàng thành công.",
                Data = true
            };
        }
    }
}

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

        public async Task<Result<object>> GetReturnRequestByArtisanIdAsync(Guid artisanId, ReturnStatusEnum? status, int pageIndex, int pageSize)
        {
            var result = _mapper.Map<List<ViewReturnRequestDTO>>(await _unitOfWork.returnRequestRepository.GetByArtisanIdAsync(artisanId, pageIndex, pageSize, status));
            return new Result<object>
            {
                Error = 0,
                Message = "Lấy danh sách yêu cầu hoàn trả hàng thành công.",
                Count = result.Count,
                Data = result
            };
        }

        public async Task<Result<bool>> RefundOrderAsync(SendRefundRequestDTO dto)
        {
            var orderItem = await _unitOfWork.orderItemRepository.GetOrderItemsByIdAsync(dto.OrderItemId);
            var returnRequest = _mapper.Map<ReturnRequest>(dto);
            var checkReturnRequest = await _unitOfWork.returnRequestRepository.GetReturnRequestByOrderItemIdAsync(dto.OrderItemId);
            var uploadResult = await _cloudinaryService.UploadProductImage(dto.ImageUrl, FOLDER);

            if(checkReturnRequest != null)
            {
                return new Result<bool>
                {
                    Error = 1,
                    Message = "Yêu cầu hoàn trả hàng đã tồn tại.",
                    Data = false
                };
            }

            if (orderItem.Status == OrderStatusEnum.Completed)
            {
                return new Result<bool>
                {
                    Error = 1,
                    Message = "Đơn hàng đã được xác nhận là nhận đơn nên không thể hoàn hàng.",
                    Data = false
                };
            }

            if (orderItem.Status != OrderStatusEnum.Delivered)
            {
                return new Result<bool>
                {
                    Error = 1,
                    Message = "Đơn hàng chưa được giao đến tay bạn nên không thể hoàn hàng.",
                    Data = false
                };
            }

            if (orderItem == null || orderItem.Order.Payment == null)
            {
                return new Result<bool>
                {
                    Error = 1,
                    Message = "Đơn hàng không tồn tại hoặc chưa thanh toán.",
                    Data = false
                };
            }

            if (orderItem.Order.Payment.IsRefunded)
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

            orderItem.Status = OrderStatusEnum.ReturnRequested;


            await _unitOfWork.SaveChangeAsync();

            return new Result<bool>
            {
                Error = 0,
                Message = "Đã gửi yêu cầu hoàn trả hàng thành công.",
                Data = true
            };
        }

        public async Task<Result<object>> UpdateStatusReturnRequestAsync(Guid returnRequestId, ReturnStatusEnum status)
        {
            var getRequest = await _unitOfWork.returnRequestRepository.GetByIdAsync(returnRequestId);
            if (getRequest == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Yêu cầu hoàn tiền khg tồn tại.",
                };
            }

            if (getRequest.Status != ReturnStatusEnum.Pending)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Yêu cầu hoàn tiền đã được xử lý trước đó.",
                };
            }

            if (getRequest.Status == ReturnStatusEnum.Approved || getRequest.Status == ReturnStatusEnum.Rejected || getRequest.Status == ReturnStatusEnum.Refunded)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Yêu cầu hoàn tiền đã được duyệt trước đó.",
                };
            }

            getRequest.Status = status;
            getRequest.ApprovedAt = DateTime.UtcNow.AddHours(7);
            getRequest.ModificationDate = DateTime.UtcNow.AddHours(7);
            _unitOfWork.returnRequestRepository.Update(getRequest);
            await _unitOfWork.SaveChangeAsync();
            return new Result<object>
            {
                Error = 0,
                Message = "Cập nhật trạng thái yêu cầu hoàn tiền thành công.",
                Data = getRequest
            };
        }
    }
}

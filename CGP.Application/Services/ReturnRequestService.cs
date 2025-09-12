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
        private readonly IOrderService _orderService;
        private static string FOLDER = "return-request";

        public ReturnRequestService(IUnitOfWork unitOfWork, IMapper mapper, IPayoutService payoutService, ICloudinaryService cloudinaryService, IOrderService orderService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _payoutService = payoutService;
            _cloudinaryService = cloudinaryService;
            _orderService = orderService;
        }

        public async Task<Result<object>> GetReturnRequestByUserIdAsync(Guid userId, ReturnStatusEnum? status, int pageIndex, int pageSize)
        {
            var result = _mapper.Map<List<ViewReturnRequestDTO>>(await _unitOfWork.returnRequestRepository.GetByUserIdAsync(userId, pageIndex, pageSize, status));
            return new Result<object>
            {
                Error = 0,
                Message = "Lấy danh sách yêu cầu hoàn trả hàng thành công.",
                Count = result.Count,
                Data = result
            };
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

        public async Task<Result<object>> GetEscalatedReturnRequestAsync(int pageIndex, int pageSize)
        {
            var result = _mapper.Map<List<ViewEscalatedDTO>>(await _unitOfWork.returnRequestRepository.GetEscalatedBAsync(pageIndex, pageSize));
            return new Result<object>
            {
                Error = 0,
                Message = "Lấy danh sách khiếu nại hoàn trả hàng thành công.",
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
            var order = await _unitOfWork.orderRepository.GetByIdAsync(orderItem.OrderId);

            // Kiểm tra yêu cầu hoàn trả đã tồn tại
            if (checkReturnRequest != null)
            {
                return new Result<bool>
                {
                    Error = 1,
                    Message = "Yêu cầu hoàn trả hàng đã tồn tại.",
                    Data = false
                };
            }

            // Kiểm tra trạng thái đơn hàng
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

            // Kiểm tra đơn hàng và thanh toán
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

            // Kiểm tra hình ảnh
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

            // Thêm yêu cầu hoàn trả
            returnRequest.ImageUrl = uploadResult.SecureUrl.ToString();
            await _unitOfWork.returnRequestRepository.AddAsync(returnRequest);

            // Cập nhật trạng thái OrderItem
            orderItem.Status = OrderStatusEnum.ReturnRequested;

            // Lấy tất cả OrderItem của Order
            var orderItems = await _unitOfWork.orderItemRepository.GetOrderItemsByOrderIdAsync(orderItem.OrderId);

            // Kiểm tra xem tất cả OrderItem có trạng thái ReturnRequested không
            bool allItemsReturned = orderItems.All(item => item.Status == OrderStatusEnum.ReturnRequested);

            // Cập nhật trạng thái Order
            if (allItemsReturned)
            {
                order.Status = OrderStatusEnum.FullReturn;
            }
            else
            {
                order.Status = OrderStatusEnum.PartialReturn;
            }

            _unitOfWork.orderRepository.Update(order);

            await _unitOfWork.SaveChangeAsync();

            return new Result<bool>
            {
                Error = 0,
                Message = "Đã gửi yêu cầu hoàn trả hàng thành công.",
                Data = true
            };
        }

        public async Task<Result<object>> UpdateStatusReturnRequestAsync(Guid returnRequestId, ReturnStatusEnum status, RejectReturnReasonEnum rejectReason)
        {
            var getRequest = await _unitOfWork.returnRequestRepository.GetReturnRequestById(returnRequestId);
            var getWalletUser = await _unitOfWork.walletRepository.GetWalletByUserIdAsync(getRequest.UserId);
            var getWalletSystem = await _unitOfWork.walletRepository.GetWalletSystem();
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

            if(status == ReturnStatusEnum.Approved)
            {
                getRequest.RejectReturnReasonEnum = null;
                getWalletSystem.PendingBalance -= getRequest.OrderItem.Order.TotalPrice;
                var addMoneyToWalletSystem = new WalletTransaction
                {
                    Wallet_Id = getWalletSystem.Id,
                    Amount = getRequest.OrderItem.Order.TotalPrice,
                    Type = WalletTransactionTypeEnum.Refund,
                    Description = @$"Hoàn trả tiền đơn hàng {getRequest.OrderItem.Order.Id} cho nghệ nhân có mức giá {getRequest.OrderItem.Order.TotalPrice}VND.",
                    CreationDate = DateTime.UtcNow.AddHours(7),
                    CreatedAt = DateTime.UtcNow.AddHours(7),
                    IsDeleted = false
                };
                await _unitOfWork.walletTransactionRepository.AddAsync(addMoneyToWalletSystem);

                getWalletUser.AvailableBalance += getRequest.OrderItem.Order.TotalPrice;
                var addMoneyToWalletUser = new WalletTransaction
                {
                    Wallet_Id = getWalletUser.Id,
                    Amount = getRequest.OrderItem.Order.TotalPrice,
                    Type = WalletTransactionTypeEnum.Refund,
                    Description = @$"Sản phẩm có mã đơn hàng chi tiết là {getRequest.OrderItemId} được hoàn tiền {getRequest.OrderItem.Order.TotalPrice}VND.",
                    CreationDate = DateTime.UtcNow.AddHours(7),
                    CreatedAt = DateTime.UtcNow.AddHours(7),
                    IsDeleted = false
                };
                await _unitOfWork.walletTransactionRepository.AddAsync(addMoneyToWalletUser);
            }
            if(status == ReturnStatusEnum.Rejected)
            {
                getRequest.RejectReturnReasonEnum = rejectReason;
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

        public async Task<Result<object>> EscalateReturnRequestAsync(Guid returnRequestId, string reason)
        {
            var request = await _unitOfWork.returnRequestRepository.GetByIdAsync(returnRequestId);

            if (request == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Yêu cầu hoàn trả không tồn tại."
                };
            }

            if (request.Status != ReturnStatusEnum.Rejected)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Chỉ có thể khiếu nại khi yêu cầu đã bị từ chối."
                };
            }

            request.Status = ReturnStatusEnum.Escalated;
            request.ModificationDate = DateTime.UtcNow.AddHours(7);
            request.Description = reason;

            _unitOfWork.returnRequestRepository.Update(request);
            await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = 0,
                Message = "Đã gửi khiếu nại lên hệ thống. Admin sẽ xem xét.",
                Data = request
            };
        }

        public async Task<Result<object>> ResolveEscalatedRequestAsync(Guid returnRequestId, bool acceptRefund)
        {
            var request = await _unitOfWork.returnRequestRepository.GetReturnRequestById(returnRequestId);
            var walletUser = await _unitOfWork.walletRepository.GetWalletByUserIdAsync(request.UserId);

            if (request == null || request.Status != ReturnStatusEnum.Escalated)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Không tìm thấy yêu cầu khiếu nại hoặc yêu cầu không hợp lệ."
                };
            }

            if (acceptRefund)
            {
                request.Status = ReturnStatusEnum.Refunded;
                request.ApprovedAt = DateTime.UtcNow.AddHours(7);
                request.OrderItem.Order.Status = OrderStatusEnum.Refunded;
                request.OrderItem.Order.Payment.IsRefunded = true;

                var walletSystem = await _unitOfWork.walletRepository.GetWalletSystem();

                walletSystem.PendingBalance -= request.OrderItem.Order.TotalPrice;
                var addMoneyToWalletSystem = new WalletTransaction
                {
                    Wallet_Id = walletSystem.Id,
                    Amount = request.OrderItem.Order.TotalPrice,
                    Type = WalletTransactionTypeEnum.Refund,
                    Description = @$"Hoàn trả tiền đơn hàng {request.OrderItem.Order.Id} cho nghệ nhân có mức giá {request.OrderItem.Order.TotalPrice}VND.",
                    CreationDate = DateTime.UtcNow.AddHours(7),
                    CreatedAt = DateTime.UtcNow.AddHours(7),
                    IsDeleted = false
                };

                walletUser.AvailableBalance += request.OrderItem.Order.TotalPrice;
                var addMoneyToWalletUser = new WalletTransaction
                {
                    Wallet_Id = walletUser.Id,
                    Amount = request.OrderItem.Order.TotalPrice,
                    Type = WalletTransactionTypeEnum.Refund,
                    Description = @$"Sản phẩm có mã đơn hàng chi tiết là {request.OrderItem.Order.Id} được hoàn tiền {request.OrderItem.Order.TotalPrice}VND.",
                    CreationDate = DateTime.UtcNow.AddHours(7),
                    CreatedAt = DateTime.UtcNow.AddHours(7),
                    IsDeleted = false
                };
                await _unitOfWork.walletTransactionRepository.AddAsync(addMoneyToWalletUser);
            }
            else
            {
                request.Status = ReturnStatusEnum.Resolved;
                request.OrderItem.Order.Status = OrderStatusEnum.Completed;
            }

            request.ModificationDate = DateTime.UtcNow.AddHours(7);
            _unitOfWork.returnRequestRepository.Update(request);
            await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = 0,
                Message = "Đã xử lý khiếu nại.",
                Data = request
            };
        }
    }
}

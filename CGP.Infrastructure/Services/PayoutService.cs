using CGP.Application;
using CGP.Application.Interfaces;
using CGP.Application.Utils;
using CGP.Contract.Abstractions.VnPayService;
using CGP.Contracts.Abstractions.Shared;
using CGP.Domain.Entities;
using CGP.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CGP.Infrastructure.Services
{
    public class PayoutService : IPayoutService
    {
        private readonly VnPaySettings _settings;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public PayoutService(IOptions<VnPaySettings> options, IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _settings = options.Value;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> CreatePaymentUrl(Guid transactionId, decimal totalAmount, HttpContext context)
        {
            var vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", "2.1.0");
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", _settings.TmnCode);

            // Tổng số tiền nhân 100 theo yêu cầu của VNPay
            vnpay.AddRequestData("vnp_Amount", ((int)(totalAmount * 100)).ToString());

            vnpay.AddRequestData("vnp_CreateDate", DateTime.UtcNow.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", context.Connection.RemoteIpAddress?.ToString());
            vnpay.AddRequestData("vnp_Locale", "vn");

            vnpay.AddRequestData("vnp_OrderInfo", $"Thanh toán đơn hàng với TransactionId {transactionId}");
            vnpay.AddRequestData("vnp_OrderType", "other");

            vnpay.AddRequestData("vnp_ReturnUrl", _settings.ReturnUrl);

            // ⚠️ Đây là thay đổi quan trọng: dùng TransactionId thay vì OrderId
            vnpay.AddRequestData("vnp_TxnRef", transactionId.ToString());

            var paymentUrl = vnpay.CreateRequestUrl(_settings.PaymentUrl, _settings.HashSecret);
            return paymentUrl;
        }


        public async Task<Result<bool>> RefundAsync(Guid orderId)
        {
            var order = await _unitOfWork.orderRepository.GetOrderByIdAsync(orderId);
            var vnpay = new VnPayLibrary();

            if (order == null || order.Payment == null)
                return new Result<bool>()
                {
                    Error = 1,
                    Message = "Đơn hàng không tồn tại hoặc chưa có thông tin thanh toán.",
                    Data = false
                };

            if (!order.Payment.IsSuccess)
                return new Result<bool>()
                {
                    Error = 1,
                    Message = "Giao dịch chưa thành công",
                    Data = false
                };

            if (order.Payment.IsRefunded)
                return new Result<bool>()
                {
                    Error = 1,
                    Message = "Giao dịch này đã hoàn tiền rồi",
                    Data = false
                };

            var config = _configuration.GetSection("VnPay");
            var refundData = new SortedDictionary<string, string>
            {
                { "vnp_RequestId", Guid.NewGuid().ToString("N") },
                { "vnp_Version", config["Version"] },
                { "vnp_Command", config["Command"] },
                { "vnp_TmnCode", config["TmnCode"] },
                { "vnp_TransactionType", "02" },
                { "vnp_TxnRef", order.Id.ToString() },
                { "vnp_Amount", ((int)(order.TotalPrice * 100)).ToString() },
                { "vnp_TransactionNo", order.Payment.TransactionNo },
                { "vnp_OrderInfo", "Hoàn tiền đơn hàng khách không nhận" },
                { "vnp_TransDate", order.Payment.CreatedAt.ToString("yyyyMMddHHmmss") },
                { "vnp_CreateBy", "System" },
                { "vnp_CreateDate", DateTime.UtcNow.ToString("yyyyMMddHHmmss") }
            };

            string rawData = string.Join("&", refundData.Select(x => $"{x.Key}={x.Value}"));
            string hashSecret = config["HashSecret"];
            refundData.Add("vnp_SecureHash", vnpay.HmacSHA512(hashSecret, rawData));

            using var client = new HttpClient();
            var content = new FormUrlEncodedContent(refundData);
            var response = await client.PostAsync(config["RefundUrl"], content);
            var resultJson = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<Dictionary<string, string>>(resultJson);
            if (result != null && result.TryGetValue("vnp_ResponseCode", out var code) && code == "00")
            {
                order.Payment.IsRefunded = true;
                order.Status = OrderStatusEnum.Refunded;
                await _unitOfWork.SaveChangeAsync();
                return new Result<bool>()
                {
                    Error = 0,
                    Message = "Hoàn tiền thành công",
                    Data = true
                };
            }

            var transaction = new Transaction()
            {
                OrderId = order.Id,
                UserId = order.UserId,
                PaymentId = order.Payment.Id,
                Amount = order.TotalPrice,
                Currency = "VND",
                PaymentMethod = order.Payment.PaymentMethod,
                TransactionStatus = TransactionStatusEnum.Refunded,
                TransactionDate = DateTime.UtcNow.AddHours(7),
                TransactionFee = 0,
                UpdatedAt = DateTime.UtcNow.AddHours(7),
                Notes = "Hoàn tiền thất bại: " + (result != null && result.TryGetValue("vnp_ResponseCode", out var responseCode) ? responseCode : "Không rõ lý do"),
                IsDeleted = false,
            };
            await _unitOfWork.transactionRepository.AddAsync(transaction);
            return new Result<bool>()
            {
                Error = 1,
                Message = "Hoàn tiền thất bại",
                Data = false
            };
        }

        public async Task<bool> ValidateReturnData(IQueryCollection query)
        {
            var vnpay = new VnPayLibrary();
            var vnp_SecureHash = query.FirstOrDefault(p => p.Key == "vnp_SecureHash").Value;
            foreach (var kv in query)
            {
                vnpay.AddResponseData(kv.Key, kv.Value);
            }

            return vnpay.ValidateSignature(vnp_SecureHash,_settings.HashSecret);
        }
    }
}

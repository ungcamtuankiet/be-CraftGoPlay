using CGP.Application.Interfaces;
using CGP.Application.Utils;
using CGP.Contract.Abstractions.VnPayService;
using CGP.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Infrastructure.Services
{
    public class PayoutService : IPayoutService
    {
        private readonly VnPaySettings _settings;
        private readonly IConfiguration _configuration;

        public PayoutService(IOptions<VnPaySettings> options, IConfiguration configuration)
        {
            _settings = options.Value;
            _configuration = configuration;
        }

        public async Task<string> CreatePaymentUrl(Order order, HttpContext context)
        {
            var vnpay = new VnPayLibrary();
            vnpay.AddRequestData("vnp_Version", "2.1.0");
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", _settings.TmnCode);
            vnpay.AddRequestData("vnp_Amount", ((int)(order.TotalPrice * 100)).ToString());
            vnpay.AddRequestData("vnp_CreateDate", DateTime.UtcNow.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", context.Connection.RemoteIpAddress?.ToString());
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", $"Thanh toan don hang {order.Id}");
            vnpay.AddRequestData("vnp_OrderType", "other");
            vnpay.AddRequestData("vnp_ReturnUrl", _settings.ReturnUrl);
            vnpay.AddRequestData("vnp_TxnRef", order.Id.ToString());

            var paymentUrl = vnpay.CreateRequestUrl(_settings.PaymentUrl, _settings.HashSecret);
            return paymentUrl;
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

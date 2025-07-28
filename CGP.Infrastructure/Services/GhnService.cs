using CGP.Application;
using CGP.Application.Interfaces;
using CGP.Contract.DTO.GHN;
using CGP.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Infrastructure.Services
{
    public class GhnService : IGhnService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly HttpClient _httpClient;
        public readonly IConfiguration config;

        public GhnService(IUnitOfWork unitOfWork, HttpClient httpClient, IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _httpClient = httpClient;
            this.config = config;
        }

        public async Task<double> CalculateShippingFeeAsync(Guid orderId, Guid artisanId, List<GhnItemDto> items)
        {
            var token = config["GHN:Token"];
            var getOrder = await _unitOfWork.orderRepository.GetByIdAsync(orderId);
            var getArtisanInfo = await _unitOfWork.artisanRequestRepository.GetByIdAsync(artisanId);

            var userAddress = await _unitOfWork.userAddressRepository.GetByIdAsync((Guid)getOrder.UserAddressId);
            if (userAddress == null || string.IsNullOrEmpty(userAddress.WardCode))
            {
                throw new Exception("Địa chỉ người dùng không hợp lệ.");
            }

            var totalWeight = getOrder.OrderItems.Sum(i => i.Product.Weight * i.Quantity);
            var totalValue = getOrder.OrderItems.Sum(i => (double)i.UnitPrice * i.Quantity);
            var length = getOrder.OrderItems.Max(i => i.Product.Length);
            var width = getOrder.OrderItems.Max(i => i.Product.Width);
            var height = getOrder.OrderItems.Sum(i => i.Product.Height);

            var body = new
            {
                service_type_id = 2,
                insurance_value = totalValue,
                coupon = (string)null,
                from_district_id = getArtisanInfo.DistrictId,
                to_district_id = userAddress.DistrictId,
                to_ward_code = userAddress.WardCode,
                height = height,
                length = length,
                weight = totalWeight,
                width = width
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/fee");
            request.Headers.Add("Token", token);
            request.Content = JsonContent.Create(body);

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception("GHN API lỗi: " + error);
            }

            var content = await response.Content.ReadAsStringAsync();
            var feeResponse = JsonConvert.DeserializeObject<GhnFeeResponse>(content);

            return feeResponse?.data?.total ?? 0;
        }
    }
}

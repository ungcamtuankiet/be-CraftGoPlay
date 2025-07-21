using CGP.Application;
using CGP.Application.Interfaces;
using CGP.Contract.DTO.GHN;
using CGP.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<int> CalculateShippingFeeAsync(Order order, List<GhnItemDto> itmes)
        {
            throw new NotImplementedException();
        }
    }
}

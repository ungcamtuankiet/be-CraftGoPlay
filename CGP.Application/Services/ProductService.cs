using AutoMapper;
using CGP.Application.Interfaces;
using CGP.Contract.DTO.Category;
using CGP.Contract.DTO.Product;
using CGP.Contracts.Abstractions.Shared;
using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRedisService _redisService;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IRedisService redisService, IMapper mapper, ICloudinaryService cloudinaryService)
        {
            _unitOfWork = unitOfWork;
            _redisService = redisService;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<Result<List<ViewProductDTO>>> GetProductsAsync()
        {
            string cacheKey = "product:list";
            var cachedData = await _redisService.GetCacheAsync<List<ViewProductDTO>>(cacheKey);

            if (cachedData != null)
            {
                return new Result<List<ViewProductDTO>>
                {
                    Error = 0,
                    Message = "Get successfully (from cache)",
                    Data = cachedData
                };
            }

            var result = _mapper.Map<List<ViewProductDTO>>(await _unitOfWork.productRepository.GetProducts());
            await _redisService.SetCacheAsync(cacheKey, result, TimeSpan.FromMinutes(10));
            return new Result<List<ViewProductDTO>>
            {
                Error = 0,
                Message = "Get successfully",
                Data = result
            };
        }

        public async Task<Result<object>> CreateProduct(ProductCreateDto request)
        {
            var uploadResult = await _cloudinaryService.UploadProductImage(request.Image);

            var product = _mapper.Map<Product>(request);
            product.ImageUrl = uploadResult.SecureUrl.ToString();

            if (request.MeterialIds != null && request.MeterialIds.Any())
            {
                var meterials = await _unitOfWork.meterialRepository.GetByIdsAsync(request.MeterialIds);

                product.Meterials = meterials;
            }

            await _unitOfWork.productRepository.CreateNewProduct(product);
            return new Result<object>
            {
                Error = 0,
                Message = "Product created successfully",
                Data = _mapper.Map<ViewProductDTO>(product)
            };
        }

        public Task<Result<object>> DeleteProduct(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Result<ViewProductDTO>> GetProductByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Result<List<ViewProductDTO>>> GetProductsBySubCategoryIdAsync(Guid subCategoryId)
        {
            throw new NotImplementedException();
        }
    }
}

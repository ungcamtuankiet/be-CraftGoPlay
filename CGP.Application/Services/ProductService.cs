using AutoMapper;
using CGP.Application.Interfaces;
using CGP.Contract.Abstractions.Shared;
using CGP.Contract.DTO.Category;
using CGP.Contract.DTO.Product;
using CGP.Contracts.Abstractions.Shared;
using CGP.Domain.Entities;
using CGP.Domain.Enums;
using CloudinaryDotNet;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        private static string FOLDER = "products"; // Thư mục trên Cloudinary, có thể thay đổi theo nhu cầu

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

        public async Task<Result<List<ViewProductDTO>>> SearchProducts(string? search, int pageIndex, int pageSize,  decimal from, decimal to, string sortOrder)
        {
            string cacheKey = $"product:search:{search}:{pageIndex}:{pageSize}:{from}:{to}:{sortOrder}";
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

            var result = _mapper.Map<List<ViewProductDTO>>(await _unitOfWork.productRepository.SearchProducts(search, pageIndex, pageSize, from, to, sortOrder));
            await _redisService.SetCacheAsync(cacheKey, result, TimeSpan.FromMinutes(10));
            return new Result<List<ViewProductDTO>>
            {
                Error = 0,
                Message = "Get successfully",
                Data = result
            };
        }

        public async Task<ResponseProductsStatus<List<ViewProductDTO>>> GetProductsByStatus(int pageIndex, int pageSize, ProductStatusEnum productStatus)
        {
            string cacheKey = $"product:status:{pageIndex}:{pageSize}:{productStatus}";
            var cachedData = await _redisService.GetCacheAsync<List<ViewProductDTO>>(cacheKey);

            if (cachedData != null)
            {
                return new ResponseProductsStatus<List<ViewProductDTO>>
                {
                    Error = 0,
                    Message = "Get successfully (from cache)",
                    Count = cachedData.Count,
                    Data = cachedData,
                    
                };
            }

            var result = _mapper.Map<List<ViewProductDTO>>(await _unitOfWork.productRepository.GetProductsByStatus(pageIndex, pageSize, productStatus));
            await _redisService.SetCacheAsync(cacheKey, result, TimeSpan.FromMinutes(10));
            return new ResponseProductsStatus<List<ViewProductDTO>>
            {
                Error = 0,
                Message = "Get successfully",
                Count = result.Count,
                Data = result,   
            };
        }

        public async Task<Result<object>> CreateProduct(ProductCreateDto request)
        {
            var uploadResult = await _cloudinaryService.UploadProductImage(request.Image, FOLDER);

            var product = _mapper.Map<Product>(request);

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

        public async Task<Result<object>> UpdateProduct(ProductUpdateDTO request)
        {
            var getProduct = await _unitOfWork.productRepository.GetProductById(request.Id);
            if (getProduct == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Product not found",
                    Data = null
                };
            }

            getProduct.Meterials ??= new List<Meterial>();

            if (request.MeterialIdsToRemove?.Any() == true)
            {
                var toRemove = getProduct.Meterials
                    .Where(m => request.MeterialIdsToRemove.Contains(m.Id))
                    .ToList();

                foreach (var meterial in toRemove)
                {
                    getProduct.Meterials.Remove(meterial);
                }
            }

            if (request.MeterialIdsToAdd?.Any() == true)
            {
                var meterialsToAdd = await _unitOfWork.meterialRepository.GetByIdsAsync(request.MeterialIdsToAdd);

                foreach (var meterial in meterialsToAdd)
                {
                    if (!getProduct.Meterials.Any(m => m.Id == meterial.Id))
                    {
                        getProduct.Meterials.Add(meterial);
                    }
                }
            }

            _mapper.Map(request, getProduct);

            await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = 0,
                Message = "Product updated successfully",
                Data = _mapper.Map<ViewProductDTO>(getProduct)
            };
        }



        public async Task<Result<object>> DeleteProduct(Guid id)
        {
            var getProduct = await _unitOfWork.productRepository.GetProductById(id);
            if (getProduct == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Product not found",
                    Data = null
                };
            }

            await _unitOfWork.productRepository.DeleteProduct(getProduct);
/*            await _cloudinaryService.DeleteImageAsync(getProduct.ImageUrl);*/
            return new Result<object>
            {
                Error = 0,
                Message = "Product deleted successfully",
                Data = null
            };
        }
    }
}

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
        private static string FOLDER = "products"; 

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
                    Message = "Lấy danh sách sản phẩm thành công (từ bộ nhớ đệm).",
                    Data = cachedData
                };
            }

            var result = _mapper.Map<List<ViewProductDTO>>(await _unitOfWork.productRepository.GetProducts());
            await _redisService.SetCacheAsync(cacheKey, result, TimeSpan.FromMinutes(10));
            return new Result<List<ViewProductDTO>>
            {
                Error = 0,
                Message = "Lấy danh sách sản phẩm thành công.",
                Data = result
            };
        }

        public async Task<Result<ViewProductDTO>> GetProductByProductId(Guid productId)
        {
            var product = await _unitOfWork.productRepository.GetProductByProductId(productId);
            if (product == null)
            {
                return new Result<ViewProductDTO>
                {
                    Error = 1,
                    Message = "Sản phẩm không tồn tại.",
                    Data = null
                };
            }

            var result = _mapper.Map<ViewProductDTO>(product);
            return new Result<ViewProductDTO>
            {
                Error = 0,
                Message = "Lấy sản phẩm bằng mã sản phẩm thành công.",
                Data = result
            };
        }


        public async Task<ResponseProductsStatus<List<ViewProductDTO>>> SearchProducts(string? search, int pageIndex, int pageSize,  decimal from, decimal to, string sortOrder)
        {
            string cacheKey = $"product:search:{search}:{pageIndex}:{pageSize}:{from}:{to}:{sortOrder}";
            var cachedData = await _redisService.GetCacheAsync<List<ViewProductDTO>>(cacheKey);

            if (cachedData != null)
            {
                return new ResponseProductsStatus<List<ViewProductDTO>>
                {
                    Error = 0,
                    Message = "Lấy danh sách sản phẩm thành công (từ bộ nhớ đệm).",
                    Count = cachedData.Count,
                    Data = cachedData
                };
            }

            var result = _mapper.Map<List<ViewProductDTO>>(await _unitOfWork.productRepository.SearchProducts(search, pageIndex, pageSize, from, to, sortOrder));
            await _redisService.SetCacheAsync(cacheKey, result, TimeSpan.FromMinutes(10));
            return new ResponseProductsStatus<List<ViewProductDTO>>
            {
                Error = 0,
                Message = "Lấy danh sách sản phẩm thành công.",
                Count = result.Count,
                Data = result
            };
        }

        public async Task<ResponseProductsStatus<List<ViewProductDTO>>> GetProductsByArtisanId(Guid artisanId, int pageIndex, int pageSize, ProductStatusEnum? productStatus)
        {
            var result = _mapper.Map<List<ViewProductDTO>>(await _unitOfWork.productRepository.GetProductsByArtisanId(artisanId, pageIndex, pageSize, productStatus));
            var getArtisan = await _unitOfWork.userRepository.GetUserById(artisanId);
            if(getArtisan == null)
            {
                return new ResponseProductsStatus<List<ViewProductDTO>>
                {
                    Error = 1,
                    Message = "Nghệ nhân không tồn tại.",
                    Count = 0,
                    Data = null
                };
            }
            return new ResponseProductsStatus<List<ViewProductDTO>>
            {
                Error = 0,
                Message = "Lấy danh sách sản phẩm của nghệ nhân thành công.",
                Count = result.Count,
                Data = result,
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
                    Message = "Lấy danh sách sản phẩm bằng trạng thái sản phẩm thành công (từ bộ nhớ đệm).",
                    Count = cachedData.Count,
                    Data = cachedData,
                    
                };
            }

            var result = _mapper.Map<List<ViewProductDTO>>(await _unitOfWork.productRepository.GetProductsByStatus(pageIndex, pageSize, productStatus));
            await _redisService.SetCacheAsync(cacheKey, result, TimeSpan.FromMinutes(10));
            return new ResponseProductsStatus<List<ViewProductDTO>>
            {
                Error = 0,
                Message = "Lấy danh sách sản phẩm bằng trạng thái sản phẩm thành công",
                Count = result.Count,
                Data = result,   
            };
        }

        public async Task<Result<object>> CreateProduct(ProductCreateDto request)
        {
            try
            {
                var product = _mapper.Map<Product>(request);

                if (request.Images != null && request.Images.Count > 5)
                {
                    return new Result<object>
                    {
                        Error = 1,
                        Message = "Chỉ được tải lên tối đa 5 ảnh.",
                        Data = null
                    };
                }

                if (request.Images != null && request.Images.Any())
                {
                    foreach (var image in request.Images)
                    {
                        var uploadResult = await _cloudinaryService.UploadProductImage(image, FOLDER);

                        if (uploadResult != null)
                        {
                            product.ProductImages.Add(new ProductImage
                            {
                                ImageUrl = uploadResult.SecureUrl.ToString()
                            });
                        }
                    }
                }

                if (request.MeterialIds != null && request.MeterialIds.Any())
                {
                    var meterials = await _unitOfWork.meterialRepository.GetByIdsAsync(request.MeterialIds);
                    product.Meterials = meterials;
                }

                await _unitOfWork.productRepository.CreateNewProduct(product);
                await _redisService.RemoveCacheAsync("product:list");
                await _redisService.RemoveByPatternAsync("product:search:*");
                return new Result<object>
                {
                    Error = 0,
                    Message = "Tạo sản phẩm thành công.",
                    Data = _mapper.Map<ViewProductDTO>(product)
                };
            }
            catch (InvalidOperationException ex)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = ex.Message
                };
            }
        }

        public async Task<Result<object>> UpdateProduct(ProductUpdateDTO request)
        {
            try
            {
                var getProduct = await _unitOfWork.productRepository.GetProductById(request.Id);
                if (getProduct == null)
                {
                    return new Result<object>
                    {
                        Error = 1,
                        Message = "Sản phẩm không tồn tại.",
                        Data = null
                    };
                }
                if (!string.IsNullOrWhiteSpace(request.Name))
                    getProduct.Name = request.Name;

                if (!string.IsNullOrWhiteSpace(request.Description))
                    getProduct.Description = request.Description;

                if (request.Price != default)
                    getProduct.Price = (decimal)request.Price;

                if (request.Quantity != default)
                    getProduct.Quantity = (int)request.Quantity;

                if (Enum.IsDefined(typeof(ProductStatusEnum), request.Status))
                    getProduct.Status = (ProductStatusEnum)request.Status;

                if (request.SubCategoryId != Guid.Empty)
                    getProduct.SubCategoryId = (Guid)request.SubCategoryId;

                if (request.Artisan_id != Guid.Empty)
                    getProduct.Artisan_id = (Guid)request.Artisan_id;

                getProduct.ProductImages ??= new List<ProductImage>();
                if (request.ImagesToAdd?.Any() == true)
                {
                    foreach (var image in request.ImagesToAdd)
                    {
                        var uploadResult = await _cloudinaryService.UploadProductImage(image, FOLDER);
                        if (uploadResult != null)
                        {
                            getProduct.ProductImages.Add(new ProductImage
                            {
                                ImageUrl = uploadResult.SecureUrl.ToString()
                            });
                        }
                    }
                }
                if (request.ImagesToRemove?.Any() == true)
                {
                    var imagesToMove = getProduct.ProductImages
                        .Where(i => request.ImagesToRemove.Contains(i.Id))
                        .ToList();
                    foreach (var image in imagesToMove)
                    {
                        await _cloudinaryService.DeleteImageAsync(image.ImageUrl);
                        await _unitOfWork.productImageRepository.RemoveImage(image);
                    }
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

                await _unitOfWork.productRepository.UpdateProduct(getProduct);
                await _unitOfWork.SaveChangeAsync();
                await _redisService.RemoveCacheAsync("product:list");
                await _redisService.RemoveByPatternAsync("product:search:*");

                return new Result<object>
                {
                    Error = 0,
                    Message = "Cập nhật sản phẩm thành công.",
                    Data = null
                };
            }
            catch (InvalidOperationException ex)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = ex.Message
                };
            }
        }



        public async Task<Result<object>> DeleteProduct(Guid id)
        {
            var getProduct = await _unitOfWork.productRepository.GetProductById(id);
            if (getProduct == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Sản phẩm không tồn tại.",
                    Data = null
                };
            }

            await _unitOfWork.productRepository.DeleteProduct(getProduct);
            if (getProduct.ProductImages?.Any() == true)
            {
                foreach (var image in getProduct.ProductImages)
                {
                    await _cloudinaryService.DeleteImageAsync(image.ImageUrl); 
                }
            }
            return new Result<object>
            {
                Error = 0,
                Message = "Xóa sản phẩm thành công.",
                Data = null
            };
        }
    }
}

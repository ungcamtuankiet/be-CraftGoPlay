using CGP.Contract.Abstractions.Shared;
using CGP.Contract.DTO.Product;
using CGP.Contracts.Abstractions.Shared;
using CGP.Domain.Entities;
using CGP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Interfaces
{
    public interface IProductService
    {
        public Task<Result<List<ViewProductDTO>>> GetProductsAsync();
        public Task<Result<ViewProductDTO>> GetProductByProductId(Guid productId);
        public Task<Result<List<ViewProductDTO>>> SearchProducts(string? search, int pageIndex, int pageSize, decimal from, decimal to, string sortOrder);
        public Task<ResponseProductsStatus<List<ViewProductDTO>>> GetProductsByArtisanId(Guid artisanId, int pageIndex, int pageSize, ProductStatusEnum productStatus);
        public Task<ResponseProductsStatus<List<ViewProductDTO>>> GetProductsByStatus(int pageIndex, int pageSize, ProductStatusEnum productStatus);
        public Task<Result<object>> CreateProduct(ProductCreateDto request);
        public Task<Result<object>> UpdateProduct(ProductUpdateDTO request);
        public Task<Result<object>> DeleteProduct(Guid id);
    }
}

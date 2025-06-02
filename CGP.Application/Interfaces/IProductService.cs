using CGP.Contract.DTO.Product;
using CGP.Contracts.Abstractions.Shared;
using CGP.Domain.Entities;
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
        public Task<Result<ViewProductDTO>> GetProductByIdAsync(Guid id);
        public Task<Result<List<ViewProductDTO>>> GetProductsBySubCategoryIdAsync(Guid subCategoryId);
        public Task<Result<object>> CreateProduct(ProductCreateDto request);
        public Task<Result<object>> DeleteProduct(Guid id);
    }
}

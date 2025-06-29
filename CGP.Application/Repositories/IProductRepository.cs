using CGP.Contract.Abstractions.Shared;
using CGP.Contract.DTO.Product;
using CGP.Domain.Entities;
using CGP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Repositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        public Task<ICollection<Product>> GetProducts();
        public Task<Product> GetProductByProductId(Guid productId);
        public Task<IList<Product>> SearchProducts(string? search, int pageIndex, int pageSize, decimal from, decimal to, string sortOrder);
        public Task<IList<Product>> GetProductsByStatus(int pageIndex, int pageSize, ProductStatusEnum productStatus);
        public Task<IList<Product>> GetProductsByArtisanId(Guid artisanId, int pageIndex, int pageSize, ProductStatusEnum productStatus);
        public Task<Product> GetProductById(Guid id);
        public Task CreateNewProduct(Product product);
        public Task UpdateProduct(Product product);
        public Task DeleteProduct(Product product);
    }
}

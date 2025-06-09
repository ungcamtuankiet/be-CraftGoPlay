using CGP.Domain.Entities;
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
        public Task<IList<Product>> SearchProducts(string? search, int pageIndex, int pageSize, decimal from, decimal to, string sortOrder);
        public Task<Product> GetProductById(Guid id);
        public Task CreateNewProduct(Product product);
        public Task UpdateProduct(Product product);
        public Task DeleteProduct(Product product);
        public Task<ICollection<Product>> GetProductsBySubCategoryId(Guid subCategoryId);
    }
}

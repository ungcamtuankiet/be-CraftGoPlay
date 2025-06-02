using CGP.Application.Interfaces;
using CGP.Application.Repositories;
using CGP.Domain.Entities;
using CGP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Infrastructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext dataContext, ICurrentTime currentTime, IClaimsService claimsService) : base(dataContext, currentTime, claimsService)
        {
            _context = dataContext;
        }

        public async Task<ICollection<Product>> GetProducts()
        {
            return await _context.Product
                .Include(x => x.User)
                .Include(x => x.Meterials)
                .Include(x => x.SubCategory).ToListAsync();
        }

        public async Task<Product> GetProductById(Guid id)
        {
            return await _context.Product
                .Include(x => x.User)
                .Include(x => x.Meterials)
                .Include(x => x.SubCategory)
                .FirstOrDefaultAsync(x => x.Id == id) ?? throw new KeyNotFoundException("Product not found");
        }

        public async Task CreateNewProduct(Product product)
        {
            await _context.Product.AddAsync(product);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateProduct(Product product)
        {
            _context.Product.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProduct(Product product)
        {
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<Product>> GetProductsBySubCategoryId(Guid subCategoryId)
        {
            return await _context.Product
                .Where(x => x.SubCategory.Id == subCategoryId)
                .Include(x => x.User)
                .Include(x => x.Meterials)
                .Include(x => x.SubCategory)
                .ToListAsync();
        }
    }
}

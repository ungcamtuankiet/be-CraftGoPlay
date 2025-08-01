﻿using CGP.Application.Interfaces;
using CGP.Application.Repositories;
using CGP.Application.Services;
using CGP.Domain.Entities;
using CGP.Domain.Enums;
using CGP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Infrastructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly AppDbContext _context;
        private readonly ICurrentTime _currentTime;

        public ProductRepository(AppDbContext dataContext, ICurrentTime currentTime, IClaimsService claimsService) : base(dataContext, currentTime, claimsService)
        {
            _context = dataContext;
            _currentTime = currentTime;
        }

        public async Task<ICollection<Product>> GetProducts()
        {
            return await _context.Product
                .Include(x => x.User)
                .Include(x => x.Meterials)
                .Include(x => x.SubCategory)
                .Include(x => x.ProductImages)
                .Include(x => x.OrderItems)
                .Include(x => x.Ratings)
                .ThenInclude(x => x.User)
                .ToListAsync();
        }

        public async Task<Product> GetProductByProductId(Guid productId)
        {
            return await _context.Product
                .Include(x => x.User)
                .Include(x => x.Meterials)
                .Include(x => x.SubCategory)
                .Include(x => x.ProductImages)
                .Include(x => x.OrderItems)
                .Include(x => x.Ratings)
                .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == productId);
        }

        public async Task<IList<Product>> SearchProducts(string? search, int pageIndex, int pageSize, decimal from, decimal to, string sortOrder)
        {
            var query = _context.Product
                .Include(x => x.User)
                .Include(x => x.Meterials)
                .Include(x => x.SubCategory)
                .Include(x => x.ProductImages)
                .Include(x => x.OrderItems)
                .Include(x => x.Ratings)
                .ThenInclude(x => x.User)
                .Where(x => x.Price >= from && x.Price <= to)
                .Where(x => x.Status == ProductStatusEnum.Active);

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x =>
                    x.Name.Contains(search) ||
                    x.Description.Contains(search) ||
                    x.User.UserName.Contains(search) ||
                    x.SubCategory.SubName.Contains(search));
            }
            
            query = (sortOrder.ToLower()) switch
            {
                ("asc") => query.OrderBy(x => x.Price),
                ("desc") => query.OrderByDescending(x => x.Price),
                _ => query.OrderBy(x => x.Price)
            };


            return await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IList<Product>> GetProductsByArtisanId(Guid artisanId, int pageIndex, int pageSize, ProductStatusEnum? productStatus)
        {
            var query = _context.Product
                .Include(x => x.User)
                .Include(x => x.Meterials)
                .Include(x => x.SubCategory)
                .Include(x => x.ProductImages)
                .Include(x => x.OrderItems)
                .Include(x => x.Ratings)
                .ThenInclude(x => x.User)
                .Where(x => x.Artisan_id == artisanId);

            if(productStatus != null)
            {
                if (!string.IsNullOrWhiteSpace(productStatus.ToString()))
                {
                    switch (productStatus.ToString().ToLower())
                    {
                        case "active":
                            query = query.Where(x => x.Status == ProductStatusEnum.Active);
                            break;
                        case "inactive":
                            query = query.Where(x => x.Status == ProductStatusEnum.InActive);
                            break;
                        case "outofstock":
                            query = query.Where(x => x.Status == ProductStatusEnum.OutOfStock);
                            break;
                        case "preorder":
                            query = query.Where(x => x.Status == ProductStatusEnum.PreOrder);
                            break;
                        case "archived":
                            query = query.Where(x => x.Status == ProductStatusEnum.Archived);
                            break;
                        case "pending":
                            query = query.Where(x => x.Status == ProductStatusEnum.Pending);
                            break;
                        case "onsale":
                            query = query.Where(x => x.Status == ProductStatusEnum.OnSale);
                            break;
                        case "backorder":
                            query = query.Where(x => x.Status == ProductStatusEnum.Backorder);
                            break;
                        default:
                            query = query.Where(p => p.Status == productStatus);
                            break;
                    }
                }
                else
                {
                    query = query = query.Where(p => p.Status == productStatus);
                }
            }
            return await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IList<Product>> GetProductsByStatus(int pageIndex, int pageSize, ProductStatusEnum productStatus)
        {
            var query = _context.Product
                .Include(x => x.User)
                .Include(x => x.Meterials)
                .Include(x => x.SubCategory)
                .Include(x => x.ProductImages)
                .Include(x => x.OrderItems)
                .Include(x => x.Ratings)
                .ThenInclude(x => x.User)
                .Where(x => x.Status == productStatus);

            if (!string.IsNullOrWhiteSpace(productStatus.ToString()))
            {
                switch (productStatus.ToString().ToLower())
                {
                    case "active":
                        query = query.Where(x => x.Status == ProductStatusEnum.Active);
                        break;
                    case "inactive":
                        query = query.Where(x => x.Status == ProductStatusEnum.InActive);
                        break;
                    case "outofstock":
                        query = query.Where(x => x.Status == ProductStatusEnum.OutOfStock);
                        break;
                    case "preorder":
                        query = query.Where(x => x.Status == ProductStatusEnum.PreOrder);
                        break;
                    case "archived":
                        query = query.Where(x => x.Status == ProductStatusEnum.Archived);
                        break;
                    case "pending":
                        query = query.Where(x => x.Status == ProductStatusEnum.Pending);
                        break;
                    case "onsale":
                        query = query.Where(x => x.Status == ProductStatusEnum.OnSale);
                        break;
                    case "backorder":
                        query = query.Where(x => x.Status == ProductStatusEnum.Backorder);
                        break;
                    default:
                        query = query.Where(x => x.Status == ProductStatusEnum.Active);
                        break;
                }
            }
            else
            {
                query = query.Where(x => x.Status == ProductStatusEnum.Active);
            }

            return await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }


        public async Task<Product> GetProductById(Guid id)
        {
            return await _context.Product
                .Include(x => x.User)
                .Include(x => x.Meterials)
                .Include(x => x.SubCategory)
                .Include(x => x.ProductImages)
                .Include(x => x.OrderItems)
                .Include(x => x.Ratings)
                .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id) ?? throw new KeyNotFoundException("Product not found");
        }

        public async Task CreateNewProduct(Product product)
        {
            product.CreationDate = _currentTime.GetCurrentTime();
            await _context.Product.AddAsync(product);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateProduct(Product product)
        {
            product.ModificationDate = _currentTime.GetCurrentTime();
            _context.Product.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProduct(Product product)
        {
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<IList<Product>> GetProductsBySubCategoryId(Guid subCategoryId, int pageIndex, int pageSize, ProductStatusEnum? productStatus)
        {
            return await _context.Product
                .Include(x => x.User)
                .Include(x => x.Meterials)
                .Include(x => x.SubCategory)
                .Include(x => x.ProductImages)
                .Include(x => x.OrderItems)
                .Include(x => x.Ratings)
                .ThenInclude(x => x.User)
                .Where(x => x.SubCategoryId == subCategoryId)
                .ToListAsync();
        }
    }
}

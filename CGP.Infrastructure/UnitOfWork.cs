using CGP.Application;
using CGP.Application.Repositories;
using CGP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private readonly IAuthRepository _authRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISubCategoryRepository _subCategoryRepository;
        private readonly ICraftVillageRepository _craftVillageRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMeterialRepository _meterialRepository;

        public UnitOfWork(AppDbContext dbContext, IAuthRepository authRepository, IUserRepository userRepository, ICategoryRepository categoryRepository, ISubCategoryRepository subCategoryRepository, ICraftVillageRepository craftVillageRepository, IProductRepository productRepository, IMeterialRepository meterialRepository)
        {
            _dbContext = dbContext;
            _authRepository = authRepository;
            _userRepository = userRepository;
            _categoryRepository = categoryRepository;
            _subCategoryRepository = subCategoryRepository;
            _craftVillageRepository = craftVillageRepository;
            _productRepository = productRepository;
            _meterialRepository = meterialRepository;
        }

        public IAuthRepository authRepository => _authRepository;
        public IUserRepository userRepository => _userRepository;
        public ICategoryRepository categoryRepository => _categoryRepository;
        public ISubCategoryRepository subCategoryRepository => _subCategoryRepository;
        public ICraftVillageRepository craftVillageRepository => _craftVillageRepository;
        public IProductRepository productRepository => _productRepository;
        public IMeterialRepository meterialRepository => _meterialRepository;

        public async Task<int> SaveChangeAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}

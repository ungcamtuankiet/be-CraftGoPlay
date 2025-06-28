using CGP.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application
{
    public interface IUnitOfWork
    {

        IAuthRepository authRepository { get; }
        IUserRepository userRepository { get; }
        ICategoryRepository categoryRepository { get; }
        ISubCategoryRepository subCategoryRepository { get; }
        ICraftVillageRepository craftVillageRepository { get; }
        IProductRepository productRepository { get; }
        IMeterialRepository meterialRepository { get; }
        IUserAddressRepository userAddressRepository { get; }
        IProductImageRepository productImageRepository { get; }
        IArtisanRequestRepository artisanRequestRepository { get; }
        ICartRepository cartRepository { get; }
        ICartItemRepository cartItemRepository { get; }
        public Task<int> SaveChangeAsync();
    }
}

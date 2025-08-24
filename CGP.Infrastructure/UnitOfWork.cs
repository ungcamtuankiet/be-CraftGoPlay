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
        private readonly IUserAddressRepository _userAddressRepository;
        private readonly IProductImageRepository _productImageRepository;
        private readonly IArtisanRequestRepository _artisanRequestRepository;
        private readonly ICartRepository _cartRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IFavouriteRepository _favouriteRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ICraftSkillRepository _craftSkillRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IPointRepository _pointRepository;
        private readonly IRatingRepository _ratingRepository;
        private readonly IReturnRequestRepository _returnRequestRepository;
        private readonly IActivityLogRepository _activityLogRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly IWalletTransactionRepository _walletTransactionRepository;
        private readonly IPointTransactionRepository _pointTransactionRepository;
        private readonly ICropRepository _cropRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IQuestRepository _questRepository;
        private readonly IUserQuestRepository _userQuestRepository;
        private readonly IVoucherRepository _voucherRepository;
        private readonly IDailyCheckInRepository _dailyCheckInRepository;
        private readonly IOrderVoucherRepository _orderVoucherRepository;
        private readonly IOrderAddressRepository _orderAddressRepository;
        private readonly IFarmlandRepository _farmlandRepository;
        private readonly IFarmlandCropRepository _farmlandCropRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IShopPriceRepository _shopPriceRepository;
        private readonly ISaleTransactionRepository _saleTransactionRepository;

        public UnitOfWork(AppDbContext dbContext, IAuthRepository authRepository, IUserRepository userRepository, ICategoryRepository categoryRepository, ISubCategoryRepository subCategoryRepository, IProductRepository productRepository, IMeterialRepository meterialRepository, ICraftVillageRepository craftVillageRepository, IUserAddressRepository userAddressRepository, IProductImageRepository productImageRepository, IArtisanRequestRepository artisanRequestRepository, ICartRepository cartRepository = null, ICartItemRepository cartItemRepository = null, IFavouriteRepository favouriteRepository = null, IPaymentRepository paymentRepository = null, IOrderItemRepository orderItemRepository = null, IOrderRepository orderRepository = null, ICraftSkillRepository craftSkillRepository = null, ITransactionRepository transactionRepository = null, IPointRepository pointRepository = null, IRatingRepository ratingRepository = null, IReturnRequestRepository returnRequestRepository = null, IActivityLogRepository activityLogRepository = null, IWalletRepository walletRepository = null, IWalletTransactionRepository walletTransactionRepository = null, IPointTransactionRepository pointTransactionRepository = null, ICropRepository cropRepository = null, IInventoryRepository inventoryRepository = null, IQuestRepository questRepository = null, IUserQuestRepository userQuestRepository = null, IVoucherRepository voucherRepository = null, IDailyCheckInRepository dailyCheckInRepository = null, IOrderVoucherRepository orderVoucherRepository = null, IOrderAddressRepository orderAddressRepository = null, IFarmlandRepository farmlandRepository = null, IFarmlandCropRepository farmlandCropRepository = null, IItemRepository itemRepository = null, IShopPriceRepository shopPriceRepository = null, ISaleTransactionRepository saleTransactionRepository = null)
        {
            _dbContext = dbContext;
            _authRepository = authRepository;
            _userRepository = userRepository;
            _categoryRepository = categoryRepository;
            _subCategoryRepository = subCategoryRepository;
            _productRepository = productRepository;
            _meterialRepository = meterialRepository;
            _craftVillageRepository = craftVillageRepository;
            _userAddressRepository = userAddressRepository;
            _productImageRepository = productImageRepository;
            _artisanRequestRepository = artisanRequestRepository;
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
            _favouriteRepository = favouriteRepository;
            _paymentRepository = paymentRepository;
            _orderItemRepository = orderItemRepository;
            _orderRepository = orderRepository;
            _craftSkillRepository = craftSkillRepository;
            _transactionRepository = transactionRepository;
            _pointRepository = pointRepository;
            _ratingRepository = ratingRepository;
            _returnRequestRepository = returnRequestRepository;
            _activityLogRepository = activityLogRepository;
            _walletRepository = walletRepository;
            _walletTransactionRepository = walletTransactionRepository;
            _pointTransactionRepository = pointTransactionRepository;
            _cropRepository = cropRepository;
            _inventoryRepository = inventoryRepository;
            _questRepository = questRepository;
            _userQuestRepository = userQuestRepository;
            _voucherRepository = voucherRepository;
            _dailyCheckInRepository = dailyCheckInRepository;
            _orderVoucherRepository = orderVoucherRepository;
            _orderAddressRepository = orderAddressRepository;
            _farmlandRepository = farmlandRepository;
            _farmlandCropRepository = farmlandCropRepository;
            _itemRepository = itemRepository;
            _shopPriceRepository = shopPriceRepository;
            _saleTransactionRepository = saleTransactionRepository;
        }

        public IAuthRepository authRepository => _authRepository;
        public IUserRepository userRepository => _userRepository;
        public ICategoryRepository categoryRepository => _categoryRepository;
        public ISubCategoryRepository subCategoryRepository => _subCategoryRepository;
        public ICraftVillageRepository craftVillageRepository => _craftVillageRepository;
        public IProductRepository productRepository => _productRepository;
        public IMeterialRepository meterialRepository => _meterialRepository;
        public IUserAddressRepository userAddressRepository => _userAddressRepository;
        public IProductImageRepository productImageRepository => _productImageRepository;
        public IArtisanRequestRepository artisanRequestRepository => _artisanRequestRepository;
        public ICartRepository cartRepository => _cartRepository;
        public ICartItemRepository cartItemRepository => _cartItemRepository;
        public IFavouriteRepository favouriteRepository => _favouriteRepository;
        public IPaymentRepository paymentRepository => _paymentRepository;
        public IOrderItemRepository orderItemRepository => _orderItemRepository;
        public IOrderRepository orderRepository => _orderRepository;
        public ICraftSkillRepository craftSkillRepository => _craftSkillRepository;
        public ITransactionRepository transactionRepository => _transactionRepository;
        public IPointRepository pointRepository => _pointRepository;
        public IRatingRepository ratingRepository => _ratingRepository;
        public IReturnRequestRepository returnRequestRepository => _returnRequestRepository;
        public IActivityLogRepository activityLogRepository => _activityLogRepository;
        public IWalletRepository walletRepository => _walletRepository;
        public IWalletTransactionRepository walletTransactionRepository => _walletTransactionRepository;
        public IPointTransactionRepository pointTransactionRepository => _pointTransactionRepository;
        public ICropRepository cropRepository => _cropRepository;
        public IInventoryRepository inventoryRepository => _inventoryRepository;
        public IQuestRepository questRepository => _questRepository;
        public IUserQuestRepository userQuestRepository => _userQuestRepository;
        public IVoucherRepository voucherRepository => _voucherRepository;
        public IDailyCheckInRepository dailyCheckInRepository => _dailyCheckInRepository;
        public IOrderVoucherRepository orderVoucherRepository => _orderVoucherRepository;
        public IOrderAddressRepository orderAddressRepository => _orderAddressRepository;
        public IFarmlandRepository farmlandRepository => _farmlandRepository;
        public IFarmlandCropRepository farmlandCropRepository => _farmlandCropRepository;
        public IItemRepository itemRepository => _itemRepository;
        public IShopPriceRepository shopPriceRepository => _shopPriceRepository;
        public ISaleTransactionRepository saleTransactionRepository => _saleTransactionRepository;

        public async Task<int> SaveChangeAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}

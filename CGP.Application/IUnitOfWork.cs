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
        IFavouriteRepository favouriteRepository { get; }
        IPaymentRepository paymentRepository { get; }
        IOrderItemRepository orderItemRepository { get; }
        IOrderRepository orderRepository { get; }
        ICraftSkillRepository craftSkillRepository { get; }
        ITransactionRepository transactionRepository { get; }
        IPointRepository pointRepository { get; }
        IRatingRepository ratingRepository { get; }
        IReturnRequestRepository returnRequestRepository { get; }
        IActivityLogRepository activityLogRepository { get; }
        IWalletRepository walletRepository { get; }
        IWalletTransactionRepository walletTransactionRepository { get; }
        IPointTransactionRepository pointTransactionRepository { get; }
        IInventoryRepository inventoryRepository { get; }
        IQuestRepository questRepository { get; }
        IUserQuestRepository userQuestRepository { get; }
        IVoucherRepository voucherRepository { get; }
        IDailyCheckInRepository dailyCheckInRepository { get; }
        IOrderVoucherRepository orderVoucherRepository { get; }
        IOrderAddressRepository orderAddressRepository { get; }
        IFarmlandRepository farmlandRepository { get; }
        IFarmlandCropRepository farmlandCropRepository { get; }
        IItemRepository itemRepository { get; }
        ISaleTransactionRepository saleTransactionRepository { get; }
        IShopPriceRepository shopPriceRepository { get; }
        IUserVoucherRepository userVoucherRepository { get; }
        public Task<int> SaveChangeAsync();
    }
}

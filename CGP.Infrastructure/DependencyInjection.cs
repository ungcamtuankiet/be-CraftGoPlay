using CGP.Application;
using CGP.Application.Interfaces;
using CGP.Application.Repositories;
using CGP.Application.Services;
using CGP.Contract.Abstractions.CloudinaryService;
using CGP.Contract.Abstractions.VnPayService;
using CGP.Domain.Entities;
using CGP.Infrastructure.Data;
using CGP.Infrastructure.Repositories;
using CGP.Infrastructure.Services;
using CloudinaryDotNet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace CGP.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            //UOW
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //Services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IOtpService, OtpService>();
            services.AddScoped<IRedisService, RedisService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IGoogleService, GoogleService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICraftVillageService, CraftVillageService>();
            services.AddScoped<ISubCategoryService, SubCategoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IMeterialService, MeterialService>();
            services.AddScoped<ICloudinaryService, CloudinaryService>();
            services.AddScoped<IArtisanRequestService, ArtisanRequestService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IFavouriteService, FavouriteService>();
            services.AddScoped<IPayoutService, PayoutService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ICraftSkillService, CraftSkillService>();
            services.AddScoped<IDashBoardService, DashBoardService>();
            services.AddScoped<IRatingService, RatingService>();
            services.AddScoped<IWalletService, WalletService>();
            services.AddScoped<IReturnRequestService, ReturnRequestService>();
            services.AddScoped<IPointService, PointService>();
            services.AddScoped<IInventoryService, InventoryService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IVoucherService, VoucherService>();
            services.AddScoped<IDailyCheckInService, DailyCheckInService>();
            services.AddScoped<IFarmlandService, FarmlandService>();
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<IShopPriceService, ShopPriceService>();
            services.AddScoped<IUserVoucherService, UserVoucherService>();
            services.AddScoped<IQuestService, QuestService>();
            services.AddScoped<IUserQuestService, UserQuestService>();

            services.AddMemoryCache();
            //Repositories
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ISubCategoryRepository, SubCategoryRepository>();
            services.AddScoped<ICraftVillageRepository, CraftVillageRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IMeterialRepository, MeterialRepository>();
            services.AddScoped<IUserAddressRepository, UserAddressRepository>();
            services.AddScoped<IProductImageRepository, ProductImageRepository>();
            services.AddScoped<IArtisanRequestRepository, ArtisanRequestRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<ICartItemRepository, CartItemRepository>();
            services.AddScoped<IFavouriteRepository, FavouriteRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<ICraftSkillRepository, CraftSkillRepository>();
            services.AddScoped<ITransactionRepository, TranSactionRepository>();
            services.AddScoped<IPointRepository, PointRepository>();
            services.AddScoped<IRatingRepository, RatingRepository>();
            services.AddScoped<IReturnRequestRepository, ReturnRequestRepository>();
            services.AddScoped<IActivityLogRepository, ActivityLogRepository>();
            services.AddScoped<IWalletRepository, WalletRepository>();
            services.AddScoped<IWalletTransactionRepository, WalletTransactionRepository>();
            services.AddScoped<IPointTransactionRepository, PointTransactionRepository>();
            services.AddScoped<IInventoryRepository, InventoryRepository>();
            services.AddScoped<IQuestRepository, QuestRepository>();
            services.AddScoped<IUserQuestRepository, UserQuestRepository>();
            services.AddScoped<IVoucherRepository, VoucherRepository>();
            services.AddScoped<IDailyCheckInRepository, DailyCheckInRepository>();
            services.AddScoped<IOrderVoucherRepository, OrderVoucherRepository>();
            services.AddScoped<IOrderAddressRepository, OrderAddressRepository>();
            services.AddScoped<IFarmlandRepository, FarmlandRepository>();
            services.AddScoped<IFarmlandCropRepository, FarmlandCropRepository>();
            services.AddScoped<ISaleTransactionRepository, SaleTransactionRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IShopPriceRepository, ShopPriceRepository>();

            //Database
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
            );

            //Cloundinary
            services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySettings"));


            //VnPay
            services.Configure<VnPaySettings>(configuration.GetSection("VnPay"));

            var redisConnectionString = configuration.GetConnectionString("Redis");
            services.AddSingleton<IConnectionMultiplexer>(_ =>
                ConnectionMultiplexer.Connect(redisConnectionString));
            return services;
        }
    }
}

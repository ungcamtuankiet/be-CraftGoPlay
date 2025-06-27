using CGP.Application;
using CGP.Application.Interfaces;
using CGP.Application.Repositories;
using CGP.Application.Services;
using CGP.Contract.Abstractions.CloudinaryService;
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

            //Database
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
            );

            //Cloundinary
            services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySettings"));
            

            var redisConnectionString = configuration.GetConnectionString("Redis");
            services.AddSingleton<IConnectionMultiplexer>(_ =>
                ConnectionMultiplexer.Connect(redisConnectionString));
            return services;
        }
    }
}

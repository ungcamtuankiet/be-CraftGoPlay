using CGP.Application.Interfaces;
using CGP.Application.Services;
using CGP.Application.Utils;
using Microsoft.Extensions.DependencyInjection;
namespace CGP.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddSingleton<ICurrentTime, CurrentTime>();
            services.AddSingleton<TokenGenerators>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            return services;
        }
    }
}

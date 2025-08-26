using CGP.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CGP.Infrastructure.Jobs
{
    public class WalletReleaseJob : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public WalletReleaseJob(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();
                var walletService = scope.ServiceProvider.GetRequiredService<IWalletService>();

                await walletService.ReleasePendingTransactionsAsync();

                try
                {
                    await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);
                }
                catch (TaskCanceledException)
                {
                }
            }
        }
    }
}

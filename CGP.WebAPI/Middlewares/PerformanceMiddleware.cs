using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CGP.WebAPI.Middlewares
{
    public class PerformanceMiddleware : IMiddleware
    {
        private readonly Stopwatch _stopwatch;
        private readonly ILogger<PerformanceMiddleware> _logger; // Use ILogger

        public PerformanceMiddleware(ILogger<PerformanceMiddleware> logger)
        {
            _stopwatch = new Stopwatch();
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            _stopwatch.Restart();
            _logger.LogInformation("Performance Tracking: Request started.");

            await next(context); // Call next middleware in the pipeline

            _stopwatch.Stop();
            _logger.LogInformation($"Performance Tracking: Request ended. Time taken: {_stopwatch.ElapsedMilliseconds} ms.");
        }
    }
}

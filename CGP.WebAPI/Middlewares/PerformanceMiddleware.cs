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
            var stopwatch = Stopwatch.StartNew();
            _logger.LogInformation("Request started for {Path}", context.Request.Path);

            try
            {
                // QUAN TRỌNG: Phải có dòng này để chuyển tiếp request
                await next(context);
            }
            finally
            {
                stopwatch.Stop();
                _logger.LogInformation("Request completed in {ElapsedMs}ms", stopwatch.ElapsedMilliseconds);
            }
        } 
    }
}

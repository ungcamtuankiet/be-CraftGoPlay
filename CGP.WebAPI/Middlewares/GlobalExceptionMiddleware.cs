using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace CGP.WebAPI.Middlewares
{
    public class GlobalExceptionMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                // todo push notification & writing log
                Console.WriteLine("GlobalExceptionMiddleware");
                Console.WriteLine(ex.Message);
                await context.Response.WriteAsync(ex.ToString());
            }
        }
    }
}

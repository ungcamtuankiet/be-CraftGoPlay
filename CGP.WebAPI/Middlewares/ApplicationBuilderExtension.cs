using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CGP.WebAPI.Middlewares
{
    public static class ApplicationBuilderExtension
    {
        public static void UseSqlTableDependency(this IApplicationBuilder app, string connectionString)
        {
        }
    }
}

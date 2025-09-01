using CGP.Application;
using CGP.Application.Interfaces;
using CGP.Infrastructure;
using CGP.Infrastructure.Data;
using CGP.WebAPI;
using CGP.WebAPI.Middlewares;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

/*var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://*:{port}");*/

{
    builder.Services
        .AddApi()
        .AddApplication()
        .AddInfrastructure(builder.Configuration);
}

//Test git action

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var key = System.Text.Encoding.ASCII.GetBytes(builder.Configuration["JwtSettings:SecretKey"]);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        ClockSkew = System.TimeSpan.Zero
    };
})
.AddCookie()
.AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
{
    options.ClientId = builder.Configuration["GoogleAPI:ClientId"];
    options.ClientSecret = builder.Configuration["GoogleAPI:SecretCode"];
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins(
    "http://localhost:5000",   
    "https://localhost:5000",  
    "http://localhost:7254",
    "https://localhost:7254",
    "https://fe-craft-go-play.vercel.app",
    "http://fe-craft-go-play.vercel.app",
    "http://localhost:5173",
    "https://localhost:5173"
)
.AllowAnyHeader()
.AllowAnyMethod()
.AllowCredentials();

        });
});



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();
builder.Services.AddSignalR();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Add Hangfire server
builder.Services.AddHangfire(config =>
{
    config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
          .UseSimpleAssemblyNameTypeSerializer()
          .UseRecommendedSerializerSettings()
          .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
          {
              CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
              SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
              QueuePollInterval = TimeSpan.Zero,
              UseRecommendedIsolationLevel = true,
              DisableGlobalLocks = true
          });
});
// Add Hangfire server
builder.Services.AddHangfireServer();

var app = builder.Build();
/*using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}*/

app.UseSwagger();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

    app.UseSwaggerUI();
}
else
{
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CrafGoPlay API V1");
        c.InjectJavascript("/custom-swagger.js");
        c.RoutePrefix = string.Empty;
    });
}

/*if (!app.Environment.IsDevelopment())
{
    // app.UseHttpsRedirection(); ❌ KHÔNG BẬT trên Render
}*/

app.UseExceptionHandler("/Error");

app.UseCors("AllowSpecificOrigin");




// Middleware for performance tracking
app.UseMiddleware<PerformanceMiddleware>();


app.UseHttpsRedirection();
// Use Global Exception Middleware 
app.UseMiddleware<GlobalExceptionMiddleware>();
// use authen
app.UseAuthentication();
app.UseMiddleware<TokenBlacklistMiddleware>();
app.UseAuthorization();

app.UseHangfireDashboard();
// Đăng ký recurring job
RecurringJob.AddOrUpdate<IWalletService>(
    "release-wallet-job",  // id job
    x => x.ReleasePendingTransactionsAsync(),
    "*/5 * * * *"          // cron expression = mỗi 5 phút chạy
);

app.MapControllers();

app.UseSqlTableDependency(builder.Configuration.GetConnectionString("DefaultConnection"));

app.Run();

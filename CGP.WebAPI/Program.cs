using CGP.Application;
using CGP.Infrastructure;
using CGP.Infrastructure.Data;
using CGP.WebAPI;
using CGP.WebAPI.Middlewares;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

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
    "https://localhost:7254"
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

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>(); 
    dbContext.Database.Migrate();
}

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


app.MapControllers();

app.UseSqlTableDependency(builder.Configuration.GetConnectionString("DefaultConnection"));


app.Run();

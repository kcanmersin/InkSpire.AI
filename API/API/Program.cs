using API.Helper;
using Core.Extensions;
using MediatR;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.OpenApi.Models;
using System.IO.Compression;
using System.Reflection;
using API.GraphQL; // 📌 GraphQL Query ve Mutation'ları eklemek için

var builder = WebApplication.CreateBuilder(args);

builder.Services.LoadCoreLayerExtension(builder.Configuration);
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddServer(new OpenApiServer
    {
        Url = "http://localhost:5256",
        Description = "Local Development Server"
    });
});

// ✅ Redis Cache
builder.Services.AddStackExchangeRedisCache(redisOptions =>
{
    string connection = builder.Configuration.GetConnectionString("Redis");
    redisOptions.Configuration = connection;
});
builder.Services.AddScoped<ICacheService, RedisCacheService>();
builder.Services.AddMemoryCache();

// ✅ CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// ✅ HTTP Sıkıştırma
builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Fastest;
});
builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Optimal;
});

// ✅ **GraphQL Servisini Doğru Şekilde Ekle**
builder.Services
    .AddGraphQLServer()
    .AddQueryType<BookQuery>()      // 📌 Query ekle
    .AddMutationType<BookMutation>(); // 📌 Mutation ekle

var app = builder.Build();

// ✅ GraphQL Middleware'i düzgün ekleyelim
app.UseRouting();
app.MapGraphQL(); // 🔹 Doğru kullanımı bu şekilde

app.UseCors("AllowAll");

// ✅ Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.UseMiddleware<API.Middleware.ActionLoggingMiddleware>();

app.MapControllers();
app.UseResponseCaching();

app.Run();

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Core.Data;
using Core.Data.Entity.User;
using MediatR;
using System.Reflection;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Builder;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Core.Services;
using InkSpire.Application.Abstractions;
using InkSpire.Infrastructure.Services;
using Core.Service.Email;
using RabbitMQ.Client;
using Core.Service.RabbitMQ;
using Microsoft.AspNetCore.SignalR;
using Core.Service.Hubs;
using Core.Service.Storage;
using Core.Service.IImageGeneration;

namespace Core.Extensions
{
    public static class CoreLayerExtensions
    {
        public static IServiceCollection LoadCoreLayerExtension(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });

            services.AddJwtAuthentication(configuration);

            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.Password.RequiredLength = 5;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.AllowedForNewUsers = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddTransient<IEmailService, EmailService>();

            var groqSection = configuration.GetSection("GroqLLM");
            services.Configure<GroqLLMSettings>(groqSection);
            var groqSettings = groqSection.Get<GroqLLMSettings>();
            services.AddHttpClient<GroqLLMService>();

            services.AddSingleton<IGroqLLM>(sp =>
            {
                var logger = sp.GetRequiredService<Microsoft.Extensions.Logging.ILogger<GroqLLMService>>();
                var httpClientFactory = sp.GetRequiredService<System.Net.Http.IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient(typeof(GroqLLMService).Name);

                return new GroqLLMService(groqSettings, httpClient, logger);
            });

            services.Configure<HuggingFaceSettings>(configuration.GetSection("HuggingFace"));
            services.AddHttpClient<IImageGenerationService, HuggingFaceImageGenerationService>()
                .ConfigureHttpClient(client =>
                {
                    client.BaseAddress = new Uri("https://api-inference.huggingface.co/");
                });

            services.AddSingleton(sp =>
            {
                var factory = new ConnectionFactory()
                {
                    HostName = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true" ? "rabbitmq-server" : "localhost",
                    Port = 5672,
                    UserName = "guest",
                    Password = "guest"
                };

                return factory.CreateConnection();
            });

            services.AddSingleton<IMessagePublisher, RabbitMQPublisher>();
            services.AddHostedService<RabbitMQConsumer>();
            services.AddHostedService<BookCreateQueueConsumer>();
            services.AddSignalR();
            //add
            //services.AddSingleton<WasabiS3Service>();

            //localstorage
            services.AddSingleton<LocalStorageService>();
            services.AddSingleton<WasabiS3StorageService>();
            //services.AddSingleton<CloudinaryStorageService>();

            services.AddScoped<CloudinaryStorageService>(); 


            return services;
        }

        public static IApplicationBuilder UseCoreLayerRecurringJobs(this IApplicationBuilder app)
        {
            return app;
        }
    }
}

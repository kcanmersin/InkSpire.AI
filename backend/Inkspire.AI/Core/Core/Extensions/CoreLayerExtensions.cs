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

namespace Core.Extensions
{
    public static class CoreLayerExtensions
    {
        public static IServiceCollection LoadCoreLayerExtension(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<IChatGroqService, ChatGroqService>();

            var mysqlConnectionString = Environment.GetEnvironmentVariable("INKSPIRE_ConnectionString")
                                 ?? configuration.GetConnectionString("DefaultConnection");

            // Add ApplicationDbContext with MySQL
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(mysqlConnectionString, ServerVersion.AutoDetect(mysqlConnectionString)));

            // Add JWT Authentication
            services.AddJwtAuthentication(configuration);

            // Add Identity
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

            // Add MediatR and FluentValidation
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // Register ChatGroqService

            return services;
        }

        public static IApplicationBuilder UseCoreLayerRecurringJobs(this IApplicationBuilder app)
        {
            return app;
        }
    }
}

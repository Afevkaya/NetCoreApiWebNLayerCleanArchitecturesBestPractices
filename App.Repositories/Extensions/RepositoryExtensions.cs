using App.Repositories.Categories;
using App.Repositories.Context;
using App.Repositories.Interceptors;
using App.Repositories.Products;
using App.Repositories.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.Repositories.Extensions;

public static class RepositoryExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        // Register the DbContext with the connection string from configuration
        services.AddDbContext<AppDbContext>(options =>
        {
            var connectionString = configuration.GetSection(ConnectionStringOption.Key).Get<ConnectionStringOption>();
            if (connectionString == null || string.IsNullOrEmpty(connectionString.PostgresSql))
            {
                throw new InvalidOperationException("Connection string is not configured.");
            }
            options.UseNpgsql(connectionString.PostgresSql,
                npgsqlOptions =>
                {
                    npgsqlOptions.MigrationsAssembly(typeof(RepositoryAssembly).Assembly.FullName);
                });
            
            options.AddInterceptors(new AuditDbContextInterceptor());
        });
        
        // Register the generic repository
        services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
        // Register specific repositories if needed
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        // Register the unit of work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
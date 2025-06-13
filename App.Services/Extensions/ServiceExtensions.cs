using System.Reflection;
using App.Services.Categories;
using App.Services.ExceptionHandlers;
using App.Services.Filters;
using App.Services.Products;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.Services.Extensions;

public static class ServiceExtensions
{
    // Eğer fluent validation hataları için kendi filter 'ını yazacaksan assembly 'i ekleyip program.cs de filter ayarlarını yapman yeterli
    // Eğer custom filter yazmayacaksan SharpGrip.FluentValidation.AutoValidation.Mvc bu pakedi ekleyip
    // builder.Services.AddFluentValidationAutoValidation(); bu kodu DI container 'a eklemen yeterli
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ApiBehaviorOptions>(options=> options.SuppressModelStateInvalidFilter = true);
        
        // Register services
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped(typeof(NotFoundFilter<,>));
        // Add FluentValidation
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        // Add AutoMapper
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        
        // Exception handling middleware
        services.AddExceptionHandler<CriticalExceptionHandler>();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        return services;
    }
}
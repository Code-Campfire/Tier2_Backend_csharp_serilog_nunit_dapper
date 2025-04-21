using CodeFire.Core.Interfaces;
using CodeFire.Infrastructure.Data;
using CodeFire.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace CodeFire.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Register database context
        services.AddSingleton<IDbConnectionFactory>(sp => 
            new SqliteConnectionFactory(configuration.GetConnectionString("DefaultConnection")!));

        // Initialize the database
        services.AddSingleton<DatabaseInitializer>();

        // Register repositories
        services.AddScoped<ITodoRepository, TodoRepository>();

        Log.Information("Infrastructure services registered");

        return services;
    }
    
    // New method to initialize the database after service provider is built
    public static void InitializeDatabase(IServiceProvider serviceProvider)
    {
        var initializer = serviceProvider.GetRequiredService<DatabaseInitializer>();
        initializer.InitializeAsync().GetAwaiter().GetResult();
        Log.Information("Database initialized");
    }
} 
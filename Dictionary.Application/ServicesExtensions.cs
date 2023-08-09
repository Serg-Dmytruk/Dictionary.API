using Dictionary.Application.Repositories.ParserRepositories;
using Dictionary.Application.Services.DictionaryServices;
using Dictionary.Data;
using Dictionary.Data.Contexts;
using Dictionary.Data.DbConnectionFactories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Dictionary.Application;

public static class ServicesExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<IDictionaryService, DictionaryService>();

        return services;
    }
    
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IParserRepository, ParserRepository>();
        
        return services;
    }
    
    public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention());
        services.AddSingleton<IDbConnectionFactory>(_ => new NpsqlConnectionFacroty(connectionString));
        services.AddScoped<DbInitializer>();
        
        return services;
    }
}
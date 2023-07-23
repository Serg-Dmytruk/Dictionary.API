using Dictionary.Application.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Dictionary.Application;

public static class ServicesExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<DictionaryRepository>();
        
        return services;
    }
}
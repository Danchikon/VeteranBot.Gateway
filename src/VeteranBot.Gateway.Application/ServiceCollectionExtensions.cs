using System.Reflection;
using Microsoft.Extensions.DependencyInjection;


namespace VeteranBot.Gateway.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMappers();
        services.AddMediatR(mediatrServiceConfiguration =>
        {
            mediatrServiceConfiguration.Lifetime = ServiceLifetime.Scoped;
            mediatrServiceConfiguration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });
        
        return services;
    }
    
    public static IServiceCollection AddMappers(this IServiceCollection services)
    {
        // services.TryAddSingleton<IUserMapper, UserMapper>();
        
        return services;
    }
}
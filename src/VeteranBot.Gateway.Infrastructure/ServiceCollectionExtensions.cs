using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using VeteranBot.Gateway.Application.Abstractions;
using VeteranBot.Gateway.Infrastructure.Implementations;
using VeteranBot.Gateway.Infrastructure.Options;
using VeteranBot.Gateway.Infrastructure.Security;

namespace VeteranBot.Gateway.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment
        )
    {
        services
            .AddOptions<PasswordHasherOptions>()
            .BindConfiguration(PasswordHasherOptions.Section);

        services
            .AddOptions<GcpOptions>()
            .BindConfiguration(GcpOptions.Section);
        
        services.TryAddSingleton<IPasswordHasher, PasswordHasher>();

        services.AddSingleton<IEventPublisher, GcpEventPublisher>();
        
        // services.AddRepositories();


        services.AddSingleton(_ => GoogleCredential.GetApplicationDefault());
        services.AddSingleton<IFileStorage, GcpFileStorage>();
        services.AddSingleton(_ => StorageClient.Create());
        services.AddPublisherServiceApiClient();

        return services;
    }
    
    public static IServiceCollection AddMongoDb(this IServiceCollection services) 
    {
        // services.TryAddSingleton(provider =>
        // {
        //     var dataSourceBuilder = provider.GetRequiredService<NpgsqlDataSourceBuilder>();
        //
        //     return new NpgsqlConnection(dataSourceBuilder.Build().ConnectionString);
        // });

        return services;
    }
}
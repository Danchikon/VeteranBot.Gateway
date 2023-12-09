using Google.Apis.Auth.OAuth2;
using Google.Cloud.PubSub.V1;
using Google.Cloud.Storage.V1;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Quartz;
using Quartz.AspNetCore;
using VeteranBot.Gateway.Application.Abstractions;
using VeteranBot.Gateway.Application.Common.Mediator;
using VeteranBot.Gateway.Application.IntegrationEvents;
using VeteranBot.Gateway.Domain.Common.Repositories;
using VeteranBot.Gateway.Domain.Entities;
using VeteranBot.Gateway.Infrastructure.Common.Repositories;
using VeteranBot.Gateway.Infrastructure.EntityConfigurations;
using VeteranBot.Gateway.Infrastructure.Implementations;
using VeteranBot.Gateway.Infrastructure.Options;

namespace VeteranBot.Gateway.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment
        )
    {
        services.AddQuartz(quartzConfigurator =>
        {
            quartzConfigurator.UseInMemoryStore();
        });
        
        services.AddQuartzServer(quartzHostedServiceOptions =>
        {
            quartzHostedServiceOptions.AwaitApplicationStarted = true;
            quartzHostedServiceOptions.WaitForJobsToComplete = true;
        });
        
        
        services
            .AddOptions<PasswordHasherOptions>()
            .BindConfiguration(PasswordHasherOptions.Section);

        services
            .AddOptions<GcpOptions>()
            .BindConfiguration(GcpOptions.Section);
        
        services.TryAddSingleton<IPasswordHasher, PasswordHasher>();

        services.AddSingleton<IEventPublisher, GcpEventPublisher>();

        services.AddMongoDb();
        services.AddRepositories();

        services.AddGcpEventsConsumer<UserCreatedIntegrationEvent>("events.user_created-sub");
        services.AddGcpEventsConsumer<UserMessageSentIntegrationEvent>("events.user_message_sent-sub");

        services.AddSingleton(_ => GoogleCredential.GetApplicationDefault());
        services.AddSingleton<IFileStorage, GcpFileStorage>();
        services.AddSingleton(_ => StorageClient.Create());
        services.AddPublisherServiceApiClient();

        return services;
    }
    
    public static IServiceCollection AddGcpEventsConsumer<TIntegrationEvent>(this IServiceCollection services, string subscriptionId) 
        where TIntegrationEvent : EventBase
    {
        services.AddHostedService<GcpEventsConsumer<TIntegrationEvent>>(provider =>
        {
            var gcpOptions = provider.GetRequiredService<IOptions<GcpOptions>>();
            var serviceScopeFactory = provider.GetRequiredService<IServiceScopeFactory>();
            var logger = provider.GetRequiredService<ILogger<GcpEventsConsumer<TIntegrationEvent>>>();
            
            var subscriptionName = SubscriptionName.FromProjectSubscription(gcpOptions.Value.ProjectId, subscriptionId);
            
            var subscriberClient = SubscriberClient.Create(subscriptionName);

            return new GcpEventsConsumer<TIntegrationEvent>(logger, serviceScopeFactory, subscriberClient);
        });

        return services;
    }
    
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        UserEntityConfiguration.Configure();
        UserGroupEntityConfiguration.Configure();
        UserMessageEntityEntityConfiguration.Configure();
        
        services.AddSingleton<IMongoCollection<UserEntity>>(provider =>
        {
            var database = provider.GetRequiredService<IMongoDatabase>();
            var collection = database.GetCollection<UserEntity>("users");
            
            return collection;
        });
        
        services.AddSingleton<IMongoCollection<UserMessageEntity>>(provider =>
        {
            var database = provider.GetRequiredService<IMongoDatabase>();
            var collection = database.GetCollection<UserMessageEntity>("user_messages");
            
            return collection;
        });
        
        services.AddSingleton<IMongoCollection<UserGroupEntity>>(provider =>
        {
            var database = provider.GetRequiredService<IMongoDatabase>();
            var collection = database.GetCollection<UserGroupEntity>("user_groups");
            
            return collection;
        });
        
        services.AddSingleton<IMongoCollection<ScheduledMessageEntity>>(provider =>
        {
            var database = provider.GetRequiredService<IMongoDatabase>();
            var collection = database.GetCollection<ScheduledMessageEntity>("scheduled_messages");
            
            return collection;
        });
        
        services.AddSingleton<IRepository<ScheduledMessageEntity>, MongoRepository<ScheduledMessageEntity>>();
        services.AddSingleton<IRepository<UserMessageEntity>, MongoRepository<UserMessageEntity>>();
        services.AddSingleton<IRepository<UserGroupEntity>, MongoRepository<UserGroupEntity>>();
        services.AddSingleton<IRepository<UserEntity>, MongoRepository<UserEntity>>();
        
        return services;
    }
    
    public static IServiceCollection AddMongoDb(this IServiceCollection services) 
    {
        services.AddSingleton<IMongoClient>(provider =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString("MongoDb");
            
            return new MongoClient(connectionString);
        });
        
        services.AddSingleton<IMongoDatabase>(provider =>
        {
            var mongoClient = provider.GetRequiredService<IMongoClient>();

            return mongoClient.GetDatabase("veteran_bot_gateway");
        });

        return services;
    }
}
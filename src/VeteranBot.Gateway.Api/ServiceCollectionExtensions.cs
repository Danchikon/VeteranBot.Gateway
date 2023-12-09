using System.Text.Json.Serialization;
using HotChocolate.Types.Pagination;
using VeteranBot.Gateway.Api.GraphQl;
using VeteranBot.Gateway.Api.GraphQl.Interceptors;

namespace VeteranBot.Gateway.Api;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApi(
        this IServiceCollection services,
        IHostEnvironment environment
        )
    {
        services.AddRestApi(environment);
        services.AddGraphQlApi<Query, Mutation>(environment);

        return services;
    }
    
    public static IServiceCollection AddRestApi(this IServiceCollection services, IHostEnvironment environment)
    {
        var mvcBuilder = services.AddControllers();
        
        mvcBuilder.AddJsonOptions(jsonOptions =>
        { 
            jsonOptions.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); 
        });
        
        if (environment.IsProduction() is false)
        {
            services.AddSwaggerGen();
        }

        return services;
    }
    
    public static IServiceCollection AddGraphQlApi<TQuery, TMutation>(this IServiceCollection services, IHostEnvironment environment) 
        where TQuery : class
        where TMutation: class
    {
        var requestExecutorBuilder = services.AddGraphQLServer();
        requestExecutorBuilder.AddHttpRequestInterceptor<ExceptionalHttpRequestInterceptor>();
        
        requestExecutorBuilder.SetPagingOptions(new PagingOptions
        {
            DefaultPageSize = 10,
            IncludeTotalCount = true,
            MaxPageSize = 100
        });
        
        requestExecutorBuilder.AddMongoDbPagingProviders();
        requestExecutorBuilder.AddMongoDbFiltering();
        requestExecutorBuilder.AddMongoDbSorting();
        requestExecutorBuilder.AddMongoDbProjections();
                
        requestExecutorBuilder.AllowIntrospection(environment.IsProduction() is false);
        requestExecutorBuilder.AddQueryType<TQuery>();
        requestExecutorBuilder.AddMutationType<TMutation>();

        requestExecutorBuilder.AddMutationConventions(applyToAllMutations: true);

        return services;
    }
}
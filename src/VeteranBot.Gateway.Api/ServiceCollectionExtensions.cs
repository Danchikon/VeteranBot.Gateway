using System.Text.Json.Serialization;
using HotChocolate.Types.Pagination;
using Microsoft.EntityFrameworkCore;
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
        services.AddGraphQlApi<Query>(environment);

        return services;
    }
    
    public static IServiceCollection AddRestApi(this IServiceCollection services, IHostEnvironment environment)
    {
        services
            .AddControllers()
            .AddJsonOptions(jsonOptions => 
            { 
                jsonOptions.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); 
            });
        
        if (environment.IsProduction() is false)
        {
            services.AddSwaggerGen();
        }

        return services;
    }
    
    public static IServiceCollection AddGraphQlApi<TQuery>(this IServiceCollection services, IHostEnvironment environment) 
    
        where TQuery : class
    {
        services
            .AddGraphQLServer()
            .AddHttpRequestInterceptor<ExceptionalHttpRequestInterceptor>()
            .SetPagingOptions(new PagingOptions
            {
                DefaultPageSize = 10,
                IncludeTotalCount = true,
                MaxPageSize = 100
            })
            .AddFiltering()
            .AddSorting()
            .AddProjections()
            .AddSpatialTypes()
            .AddSpatialFiltering()
            .AddSpatialProjections()
            .AllowIntrospection(environment.IsProduction() is false)
            .AddQueryType<TQuery>();

        return services;
    }
}
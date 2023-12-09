using System.Text.Json;
using System.Text.Json.Serialization;
using Google.Cloud.PubSub.V1;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VeteranBot.Gateway.Application.Common.Mediator;

namespace VeteranBot.Gateway.Infrastructure.Implementations;
 
public class GcpEventsConsumer<TIntegrationEvent>(
    ILogger<GcpEventsConsumer<TIntegrationEvent>> logger,
    IServiceScopeFactory serviceScopeFactory,
    SubscriberClient subscriberClient
    ) : BackgroundService where TIntegrationEvent: EventBase
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = false,
        Converters = { new JsonStringEnumConverter() }
    };
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await subscriberClient.StartAsync(async (message, token) =>
        {
            await using var scope = serviceScopeFactory.CreateAsyncScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            try
            {
                var integrationEvent = JsonSerializer.Deserialize<TIntegrationEvent>(message.Data.ToByteArray(), _jsonSerializerOptions);

                if (integrationEvent is not null)
                {
                    await mediator.Publish(integrationEvent, token);
                }
            }
            catch (Exception exception)
            {
                logger.LogError("Exception in subscriber | exception - {Exception}", exception);
            }
            
            return SubscriberClient.Reply.Ack;
        });
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await subscriberClient.StopAsync(cancellationToken);
    }
}
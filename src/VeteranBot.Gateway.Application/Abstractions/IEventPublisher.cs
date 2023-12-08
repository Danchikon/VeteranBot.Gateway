namespace VeteranBot.Gateway.Application.Abstractions;

public interface IEventPublisher
{
    Task PublishAsync<TIntegrationEvent>(
        string topic, 
        TIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default
        );
}
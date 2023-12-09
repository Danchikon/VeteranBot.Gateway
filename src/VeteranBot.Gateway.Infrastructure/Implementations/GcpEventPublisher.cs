using System.Text.Json;
using System.Text.Json.Serialization;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.PubSub.V1;
using Google.Protobuf;
using Microsoft.Extensions.Options;
using VeteranBot.Gateway.Application.Abstractions;
using VeteranBot.Gateway.Infrastructure.Options;

namespace VeteranBot.Gateway.Infrastructure.Implementations;

public class GcpEventPublisher(
    PublisherServiceApiClient publisherServiceApiClient,
    IOptions<GcpOptions> gcpOptions
    ) : IEventPublisher
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = false,
        Converters = { new JsonStringEnumConverter() }
    };
    
    public async Task PublishAsync<TIntegrationEvent>(
        string topic, 
        TIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default
        )
    {
        var bytes = JsonSerializer.SerializeToUtf8Bytes(integrationEvent, _jsonSerializerOptions);
        
        var message = new PubsubMessage
        {
            Data = ByteString.CopyFrom(bytes),
        };

        var topicName = TopicName.FromProjectTopic(gcpOptions.Value.ProjectId, topic);
        
        await publisherServiceApiClient.PublishAsync(topicName, new [] { message });
    }
}
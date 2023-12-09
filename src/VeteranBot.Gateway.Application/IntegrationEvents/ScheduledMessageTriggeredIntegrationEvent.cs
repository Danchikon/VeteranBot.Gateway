namespace VeteranBot.Gateway.Application.IntegrationEvents;

public record ScheduledMessageTriggeredIntegrationEvent
{
    public IEnumerable<string> PhoneNumbers { get; init; } = Enumerable.Empty<string>();
    public required string Text { get; init; }
}

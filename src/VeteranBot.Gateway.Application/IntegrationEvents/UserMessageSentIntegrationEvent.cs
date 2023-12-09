using VeteranBot.Gateway.Application.Common.Mediator;

namespace VeteranBot.Gateway.Application.IntegrationEvents;

public record UserMessageSentIntegrationEvent : EventBase
{
    public string BotType { get; init; } = "Telegram";
    public required string BotMessageId { get; init; }
    public required string PhoneNumber { get; init; }
    public bool IsQuestion { get; init; }
    public string? Text { get; init; }
    public required DateTime Timestamp { get; init; }
}

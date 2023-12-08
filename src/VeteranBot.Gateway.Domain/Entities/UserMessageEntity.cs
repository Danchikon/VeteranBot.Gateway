using VeteranBot.Gateway.Domain.Enums;

namespace VeteranBot.Gateway.Domain.Entities;

public class UserMessageEntity
{
    public required string MessageId { get; init; }
    public required string Text { get; init; }
    public required BotType BotType { get; init; }
    public required DateTime Timestamp { get; init; }
}
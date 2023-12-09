using VeteranBot.Gateway.Domain.Enums;

namespace VeteranBot.Gateway.Domain.Entities;

public class UserMessageEntity
{
    public required Guid Id { get; init; }
    public required string UserPhoneNumber { get; init; }
    public required string BotMessageId { get; init; }
    public string? Text { get; init; }
    public bool IsQuestion { get; set; }
    public required string BotType { get; init; }
    public required DateTime Timestamp { get; init; }
}
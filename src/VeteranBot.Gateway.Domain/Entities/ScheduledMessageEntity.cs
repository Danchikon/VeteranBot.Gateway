using VeteranBot.Gateway.Domain.Enums;

namespace VeteranBot.Gateway.Domain.Entities;

public class ScheduledMessageEntity
{
    public required Guid Id { get; init; }
    public required string[] PhoneNumbers { get; set; } = Array.Empty<string>();
    public required string Text { get; set; }
    public required DateTime TriggerAt { get; init; }
    public bool IsTriggered { get; set; } = false;
}
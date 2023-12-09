using VeteranBot.Gateway.Application.Common.Mediator;

namespace VeteranBot.Gateway.Application.Commands.Messages;

public record ScheduleMessageCommand : CommandBase
{
    public required string[] PhoneNumbers { get; set; } = Array.Empty<string>();
    public required string Text { get; set; }
    public required DateTime TriggerAt { get; init; }
}
namespace VeteranBot.Gateway.Application.IntegrationEvents;

public record BroadCastMessageRequested
{
    public IEnumerable<string> PhoneNumbers { get; init; } = Enumerable.Empty<string>();
    public string? FileAttachmentName { get; init; }
    public required string Text { get; init; }
}

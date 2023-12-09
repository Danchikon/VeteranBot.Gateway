namespace VeteranBot.Gateway.Infrastructure.Options;

public record GcpOptions
{
    public const string Section = "Gcp";
    
    public required string ProjectId { get; init; }
}
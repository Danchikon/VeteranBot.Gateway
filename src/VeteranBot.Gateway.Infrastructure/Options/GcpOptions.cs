namespace VeteranBot.Gateway.Infrastructure.Options;

public class GcpOptions
{
    public const string Section = "Gcp";
    
    public required string ProjectId { get; init; }
}
using VeteranBot.Gateway.Domain.Common.Errors;

namespace VeteranBot.Gateway.Api.Dtos;

public record ErrorDto
{
    public required ErrorCode Code { get; init; }
    public required ErrorKind Kind { get; init; }
    public ICollection<string> Messages { get; init; } = new List<string>();
};
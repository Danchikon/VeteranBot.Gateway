namespace VeteranBot.Gateway.Api.Dtos;

public record RequestBroadCastMessageRequestDto
{
    public IFormFile? FileAttachment { get; init; }
    public required IEnumerable<string> PhoneNumbers { get; init; }
    public required string Text { get; init; }
}
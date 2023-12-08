using VeteranBot.Gateway.Application.Common.Mediator;
using VeteranBot.Gateway.Application.Dtos;

namespace VeteranBot.Gateway.Application.Commands.Messages;

public record RequestBroadCastMessageCommand : CommandBase
{
    public required IEnumerable<string> PhoneNumbers { get; init; } 
    public FileDto? FileAttachment { get; init; } 
    public required string Text { get; init; }
};
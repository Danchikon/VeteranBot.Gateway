using VeteranBot.Gateway.Application.Common.Mediator;
using VeteranBot.Gateway.Domain.Enums;

namespace VeteranBot.Gateway.Application.IntegrationEvents;

public record UserCreatedIntegrationEvent : EventBase
{
    public required string BotType { get; init; }
    public required string PhoneNumber { get; init; }
    public required string FullName { get; init; }
    public required int Age { get; init; }
    public required string Region { get; init; }
    public required UserType Type { get; init; }
    public DateTime RegistrationDate { get; init; }
}

using VeteranBot.Gateway.Application.Common.Mediator;
using VeteranBot.Gateway.Domain.Entities;

namespace VeteranBot.Gateway.Application.Commands.UserGroups;

public record DeleteUserGroupCommand : CommandBase
{
    public required Guid Id { get; init; }
}
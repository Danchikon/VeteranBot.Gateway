using VeteranBot.Gateway.Application.Common.Mediator;
using VeteranBot.Gateway.Domain.Entities;

namespace VeteranBot.Gateway.Application.Commands.UserGroups;

public record UpdateUserGroupCommand : CommandBase<UserGroupEntity>
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public ICollection<string> UsersPhoneNumbers { get; init; } = Array.Empty<string>();
}
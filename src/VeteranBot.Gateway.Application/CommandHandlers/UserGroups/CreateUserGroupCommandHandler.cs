using VeteranBot.Gateway.Application.Commands.UserGroups;
using VeteranBot.Gateway.Application.Common.Mediator;
using VeteranBot.Gateway.Domain.Common.Repositories;
using VeteranBot.Gateway.Domain.Entities;

namespace VeteranBot.Gateway.Application.CommandHandlers.UserGroups;

public class CreateUserGroupCommandHandler(
    IRepository<UserGroupEntity> userGroupsRepository
    ) : CommandHandlerBase<CreateUserGroupCommand, UserGroupEntity>
{
    public override async Task<UserGroupEntity> Handle(CreateUserGroupCommand command, CancellationToken cancellationToken)
    {
        var userGroup = new UserGroupEntity
        {
            Id = Guid.NewGuid(),
            Title = command.Title,
            UsersPhoneNumbersCount = command.UsersPhoneNumbers.Count,
            UsersPhoneNumbers = command.UsersPhoneNumbers,
            CreatedAt = DateTime.UtcNow
        };

        var updatedUserGroup = await userGroupsRepository.AddAsync(userGroup, cancellationToken);

        return updatedUserGroup;
    }
}
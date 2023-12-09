using VeteranBot.Gateway.Application.Commands.UserGroups;
using VeteranBot.Gateway.Application.Common.Mediator;
using VeteranBot.Gateway.Domain.Common.Repositories;
using VeteranBot.Gateway.Domain.Entities;

namespace VeteranBot.Gateway.Application.CommandHandlers.UserGroups;

public class UpdateUserGroupCommandHandler(
    IRepository<UserGroupEntity> userGroupsRepository
    ) : CommandHandlerBase<UpdateUserGroupCommand, UserGroupEntity>
{
    public override async Task<UserGroupEntity> Handle(UpdateUserGroupCommand command, CancellationToken cancellationToken)
    {
        var userGroup = new UserGroupEntity
        {
            Id = command.Id,
            Title = command.Title,
            UsersPhoneNumbersCount = command.UsersPhoneNumbers.Count,
            UsersPhoneNumbers = command.UsersPhoneNumbers,
            CreatedAt = DateTime.UtcNow
        };

        var updatedUserGroup = await userGroupsRepository.UpdateAsync(group => group.Id == command.Id, userGroup, cancellationToken);

        return updatedUserGroup;
    }
}
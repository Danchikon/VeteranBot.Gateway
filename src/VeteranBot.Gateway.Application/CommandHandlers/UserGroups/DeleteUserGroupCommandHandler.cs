using VeteranBot.Gateway.Application.Commands.UserGroups;
using VeteranBot.Gateway.Application.Common.Mediator;
using VeteranBot.Gateway.Domain.Common.Repositories;
using VeteranBot.Gateway.Domain.Entities;

namespace VeteranBot.Gateway.Application.CommandHandlers.UserGroups;

public class DeleteUserGroupCommandHandler(
    IRepository<UserGroupEntity> userGroupsRepository
    ) : CommandHandlerBase<DeleteUserGroupCommand>
{
    public override async Task Handle(DeleteUserGroupCommand command, CancellationToken cancellationToken)
    {
        await userGroupsRepository.RemoveAsync(group => group.Id == command.Id, cancellationToken);
    }
}
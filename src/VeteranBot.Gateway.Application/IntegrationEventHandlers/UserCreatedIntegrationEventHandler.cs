using System.Linq.Expressions;
using VeteranBot.Gateway.Application.Common.Mediator;
using VeteranBot.Gateway.Application.IntegrationEvents;
using VeteranBot.Gateway.Domain.Common.Repositories;
using VeteranBot.Gateway.Domain.Entities;

namespace VeteranBot.Gateway.Application.IntegrationEventHandlers;

public class UserCreatedIntegrationEventHandler(IRepository<UserEntity> usersRepository) : EventHandlerBase<UserCreatedIntegrationEvent>
{
    public override async Task Handle(UserCreatedIntegrationEvent @event, CancellationToken cancellationToken)
    {
        var user = await usersRepository.FirstOrDefaultAsync(
            new Expression<Func<UserEntity, bool>>[]
            {
                entity => entity.PhoneNumber == @event.PhoneNumber
            }, 
            cancellationToken
            )
            ;

        if (user is not null && user.BotTypes.Contains(@event.BotType))
        {
            return;
        }
        
        user = new UserEntity
        {
            Age = @event.Age,
            FullName = @event.FullName,
            Type = @event.Type,
            PhoneNumber = @event.PhoneNumber,
            RegistrationDate = @event.RegistrationDate,
            Region = @event.Region,
            BotTypes = new [] { @event.BotType }
        };

        await usersRepository.AddAsync(user, cancellationToken);
    }
}
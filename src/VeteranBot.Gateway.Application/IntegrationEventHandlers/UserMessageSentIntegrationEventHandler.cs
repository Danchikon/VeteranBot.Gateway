using System.Linq.Expressions;
using VeteranBot.Gateway.Application.Common.Mediator;
using VeteranBot.Gateway.Application.IntegrationEvents;
using VeteranBot.Gateway.Domain.Common.Repositories;
using VeteranBot.Gateway.Domain.Entities;

namespace VeteranBot.Gateway.Application.IntegrationEventHandlers;

public class UserMessageSentIntegrationEventHandler(
    IRepository<UserMessageEntity> userMessagesRepository
    ) : EventHandlerBase<UserMessageSentIntegrationEvent>
{
    public override async Task Handle(UserMessageSentIntegrationEvent @event, CancellationToken cancellationToken)
    {
        var userMessage = new UserMessageEntity
        {
            Id = Guid.NewGuid(),
            IsQuestion = @event.IsQuestion,
            BotMessageId = @event.BotMessageId,
            Timestamp = @event.Timestamp,
            UserPhoneNumber = @event.PhoneNumber,
            Text = @event.Text,
            BotType = @event.BotType 
        };

        await userMessagesRepository.AddAsync(userMessage, cancellationToken);
    }
}
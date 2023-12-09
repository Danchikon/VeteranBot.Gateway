using System.Linq.Expressions;
using Quartz;
using VeteranBot.Gateway.Application.Abstractions;
using VeteranBot.Gateway.Application.IntegrationEvents;
using VeteranBot.Gateway.Domain.Common.Repositories;
using VeteranBot.Gateway.Domain.Entities;

namespace VeteranBot.Gateway.Infrastructure.Implementations;

public class SendMessageJob(
    IEventPublisher eventPublisher,
    IRepository<ScheduledMessageEntity> scheduledMessagesRepository
    ) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        var scheduledMessageId = context.JobDetail.JobDataMap.GetGuidValue("scheduled_message_id");

        var scheduledMessage = await scheduledMessagesRepository.FirstAsync(
            new Expression<Func<ScheduledMessageEntity, bool>>[]
            {
                message => message.Id == scheduledMessageId
            },
            context.CancellationToken
            );

        var scheduledMessageTriggeredEvent = new ScheduledMessageTriggeredIntegrationEvent
        {
            Text = scheduledMessage.Text,
            PhoneNumbers = scheduledMessage.PhoneNumbers
        };

        await eventPublisher.PublishAsync("events.scheduled_message_triggered", scheduledMessageTriggeredEvent, context.CancellationToken);

        scheduledMessage.IsTriggered = true;
        
        await scheduledMessagesRepository.UpdateAsync(
            message => message.Id == scheduledMessageId,
            scheduledMessage,
            context.CancellationToken
        );
    }
}
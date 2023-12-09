using MediatR;
using Quartz;
using VeteranBot.Gateway.Application.Commands.Messages;
using VeteranBot.Gateway.Application.Commands.UserGroups;
using VeteranBot.Gateway.Domain.Common.Repositories;
using VeteranBot.Gateway.Domain.Entities;
using VeteranBot.Gateway.Infrastructure.Implementations;

namespace VeteranBot.Gateway.Api.GraphQl;

public class Mutation
{
    public async Task<UserGroupEntity> CreateUserGroup(
        CreateUserGroupCommand command, 
        [Service] IMediator mediator,
        CancellationToken cancellationToken
        )
    {
        return await mediator.Send(command, cancellationToken);
    } 
    
    public async Task<UserGroupEntity> UpdateUserGroup(
        UpdateUserGroupCommand command, 
        [Service] IMediator mediator,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(command, cancellationToken);
    } 
    
    public async Task<bool> DeleteUserGroup(
        DeleteUserGroupCommand command, 
        [Service] IMediator mediator,
        CancellationToken cancellationToken
    )
    {
        await mediator.Send(command, cancellationToken);

        return true;
    } 
    
    public async Task<bool> RescheduledMessage(
        RescheduleMessageCommand command, 
        [Service] ISchedulerFactory schedulerFactory,
        [Service] IRepository<ScheduledMessageEntity> scheduledMessagesRepository,
        CancellationToken cancellationToken
    )
    {
        var scheduledMessage = new ScheduledMessageEntity
        {
            Id = command.Id,
            Text = command.Text,
            PhoneNumbers = command.PhoneNumbers,
            TriggerAt = command.TriggerAt
        };

        await scheduledMessagesRepository.UpdateAsync(
            message => message.Id == command.Id, 
            scheduledMessage,
            cancellationToken
            );
        
        var scheduler = await schedulerFactory.GetScheduler(cancellationToken);
        
        var trigger = TriggerBuilder.Create()
            .WithIdentity($"{nameof(SendMessageJob)}-trigger-{scheduledMessage.Id}")
            .StartAt(scheduledMessage.TriggerAt)
            .Build();
        
        await scheduler.RescheduleJob(trigger.Key, trigger, cancellationToken);
        
        return true;
    } 
    
    public async Task<bool> ResetScheduledMessage(
        Guid id, 
        [Service] ISchedulerFactory schedulerFactory,
        [Service] IRepository<ScheduledMessageEntity> scheduledMessagesRepository,
        CancellationToken cancellationToken
    )
    {
        await scheduledMessagesRepository.RemoveAsync(message => message.Id == id, cancellationToken);
        
        var scheduler = await schedulerFactory.GetScheduler(cancellationToken);

        var triggerKey = new TriggerKey($"{nameof(SendMessageJob)}-trigger-{id}");
        var jobKey = new JobKey($"{nameof(SendMessageJob)}-job-{id}");

        await scheduler.ResumeTrigger(triggerKey, cancellationToken);
        await scheduler.ResumeJob(jobKey, cancellationToken);
        
        return true;
    } 
    
    public async Task<bool> ScheduleMessage(
        ScheduleMessageCommand command, 
        [Service] ISchedulerFactory schedulerFactory,
        [Service] IRepository<ScheduledMessageEntity> scheduledMessagesRepository,
        CancellationToken cancellationToken
    )
    {
        var scheduledMessage = new ScheduledMessageEntity
        {
            Id = Guid.NewGuid(),
            Text = command.Text,
            PhoneNumbers = command.PhoneNumbers,
            TriggerAt = command.TriggerAt
        };

        await scheduledMessagesRepository.AddAsync(scheduledMessage, cancellationToken);
        
        var scheduler = await schedulerFactory.GetScheduler(cancellationToken);
        
        var job = JobBuilder.Create<SendMessageJob>()
            .WithIdentity($"{nameof(SendMessageJob)}-job-{scheduledMessage.Id}")
            .UsingJobData("scheduled_message_id", scheduledMessage.Id)
            .Build();
        
        var trigger = TriggerBuilder.Create()
            .WithIdentity($"{nameof(SendMessageJob)}-trigger-{scheduledMessage.Id}")
            .StartAt(scheduledMessage.TriggerAt)
            .Build();

        await scheduler.ScheduleJob(job, trigger, cancellationToken);
        
        return true;
    } 
}
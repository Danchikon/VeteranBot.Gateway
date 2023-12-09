using MediatR;

namespace VeteranBot.Gateway.Application.Common.Mediator;

public abstract class EventHandlerBase<TEvent> : INotificationHandler<TEvent> where TEvent : EventBase
{
    public abstract Task Handle(TEvent @event, CancellationToken cancellationToken);
}
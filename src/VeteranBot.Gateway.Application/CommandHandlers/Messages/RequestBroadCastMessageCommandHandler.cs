using VeteranBot.Gateway.Application.Abstractions;
using VeteranBot.Gateway.Application.Commands.Messages;
using VeteranBot.Gateway.Application.Common.Mediator;
using VeteranBot.Gateway.Application.IntegrationEvents;

namespace VeteranBot.Gateway.Application.CommandHandlers.Messages;

public class RequestBroadCastMessageCommandHandler(
    IEventPublisher eventPublisher, 
    IFileStorage fileStorage
    ) : CommandHandlerBase<RequestBroadCastMessageCommand>
{
    public override async Task Handle(RequestBroadCastMessageCommand command, CancellationToken cancellationToken)
    {
        var integrationEvent = new BroadCastMessageRequested
        {
            PhoneNumbers = command.PhoneNumbers,
            Text = command.Text
        };

        if (command.FileAttachment is {} fileAttachment)
        {
            var fileName = Guid.NewGuid().ToString();
            
            await fileStorage.UploadAsync(
                "file_attachments", 
                fileAttachment with { Name = fileName },
                cancellationToken
                );

            integrationEvent = integrationEvent with
            {
                FileAttachmentName = fileName
            };
        }

        await eventPublisher.PublishAsync("events.broadcast_message_requested",  integrationEvent, cancellationToken);
    }
};
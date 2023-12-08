using MediatR;
using Microsoft.AspNetCore.Mvc;
using VeteranBot.Gateway.Api.Dtos;
using VeteranBot.Gateway.Application.Commands.Messages;
using VeteranBot.Gateway.Application.Dtos;

namespace VeteranBot.Gateway.Api.Rest.Controllers;

[ApiController]
[Route("api/messages")]
public class MessagesController : ControllerBase
{
   
    private readonly IMediator _mediator;

    public MessagesController(
        IMediator mediator
        )
    {
        _mediator = mediator;
    }
    
    [HttpPost("broadcast/request")]
    public async Task<ActionResult> RequestBroadcast(
        [FromForm] RequestBroadCastMessageRequestDto dto,
        CancellationToken cancellationToken
        )
    {
        var command = new RequestBroadCastMessageCommand
        {
            PhoneNumbers = dto.PhoneNumbers,
            Text = dto.Text
        };

        if (dto.FileAttachment is {} fileAttachment)
        {
            var fileDto = new FileDto
            {
                Stream = fileAttachment.OpenReadStream(),
                ContentType = fileAttachment.ContentType,
                Name = fileAttachment.FileName
            };

            command = command with
            {
                FileAttachment = fileDto
            };
        }
        
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    } 
}
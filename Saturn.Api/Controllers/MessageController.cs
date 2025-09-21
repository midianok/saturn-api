using MediatR;
using Microsoft.AspNetCore.Mvc;
using Saturn.Application.Dtos;
using Saturn.Application.Features.SaveMessage;

namespace Saturn.Api.Controllers;

[ApiController]
[Route("message")]
public class MessageController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Save telegram message
    /// </summary>
    /// <param name="saveMessageDto"></param>
    /// <returns></returns>
    [HttpPost(Name = "SaveMessage")]
    public Task SaveMessageAsync(SaveMessageDto saveMessageDto) => 
        mediator.Send(new SaveMessageCommand(saveMessageDto));
}
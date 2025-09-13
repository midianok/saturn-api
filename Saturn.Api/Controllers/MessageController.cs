using MediatR;
using Microsoft.AspNetCore.Mvc;
using Saturn.Application.Dtos;
using Saturn.Application.Features.SaveMessage;

namespace Saturn.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class MessageController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public Task SaveMessageAsync(SaveMessageDto saveMessageDto) => 
        mediator.Send(new SaveMessageCommand(saveMessageDto));
}
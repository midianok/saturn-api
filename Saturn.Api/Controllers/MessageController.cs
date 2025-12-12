using MediatR;
using Microsoft.AspNetCore.Mvc;
using Saturn.Application.Dtos;
using Saturn.Application.Features.GetUserMessageStatistics;
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
        mediator.Send(new SaveMessageCommand(saveMessageDto), HttpContext.RequestAborted);
    
    [HttpGet(Name = "GetUserMessageStatistics")]
    public Task<UserMessageStatisticsResponseDto> GetUserMessageStatisticsAsync(long userId, long chatId) => 
        mediator.Send(new GetUserMessageStatisticsQuery(chatId, userId), HttpContext.RequestAborted);
}
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Saturn.Application.Dtos;
using Saturn.Application.Features.GetChatMessageStatistics;
using Saturn.Application.Features.GetFavSticker;
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
    [HttpPost("save-message")]
    public Task SaveMessageAsync(SaveMessageDto saveMessageDto) => 
        mediator.Send(new SaveMessageCommand(saveMessageDto), HttpContext.RequestAborted);


    /// <summary>
    /// Get user message statistics for a specific chat
    /// </summary>
    /// <param name="userId">The Telegram user ID</param>
    /// <param name="chatId">The Telegram chat ID</param>
    /// <param name="from">Optional start date to filter messages from (inclusive)</param>
    /// <param name="to">Optional end date to filter messages to (inclusive)</param>
    /// <returns>Statistics containing message counts by type for the specified user in the chat</returns>
    [HttpGet("get-user-message-statistics")]
    public Task<GetUserMessageStatisticsResponseDto> GetUserMessageStatisticsAsync(long userId, long chatId, DateTime? from, DateTime? to) => 
        mediator.Send(new GetUserMessageStatisticsQuery(chatId, userId, from, to), HttpContext.RequestAborted);
    
    
    /// <summary>
    /// Get message statistics for all users in a specific chat
    /// </summary>
    /// <param name="chatId">The Telegram chat ID</param>
    /// <param name="from">Optional start date to filter messages from (inclusive)</param>
    /// <param name="to">Optional end date to filter messages to (inclusive)</param>
    /// <returns>Statistics containing message counts by type for all users in the specified chat</returns>
    [HttpGet("get-chat-message-statistics")]
    public Task<GetChatMessageStatisticsResponseDto> GetChatMessageStatisticsAsync(long chatId, DateTime? from, DateTime? to) => 
        mediator.Send(new GetChatMessageStatisticsQuery(chatId, from, to), HttpContext.RequestAborted);

    [HttpGet("get-fav-sticker")]
    public Task<GetFavStickerResponseDto> GetFavStickerAsync(long userId, long chatId) => 
        mediator.Send(new GetFavStickerQuery(userId, chatId), HttpContext.RequestAborted);
}
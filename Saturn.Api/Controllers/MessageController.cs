using Microsoft.AspNetCore.Mvc;
using Saturn.Application.Dtos;
using Saturn.Application.Mappers;
using Saturn.Domain.Model;
using Saturn.Domain.Services;

namespace Saturn.Api.Controllers;

[ApiController]
[Route("message")]
public class MessageController(IMessageService messageService) : ControllerBase
{
    /// <summary>
    /// Save telegram message
    /// </summary>
    /// <param name="saveMessageDto"></param>
    /// <returns></returns>
    [HttpPost("save-message")]
    public Task SaveMessageAsync(SaveMessageDto saveMessageDto) => 
        messageService.SaveMessageAsync(saveMessageDto.ToTelegramMessage(), HttpContext.RequestAborted);


    /// <summary>
    /// Get user message statistics for a specific chat
    /// </summary>
    /// <param name="userId">The Telegram user ID</param>
    /// <param name="chatId">The Telegram chat ID</param>
    /// <param name="from">Optional start date to filter messages from (inclusive)</param>
    /// <param name="to">Optional end date to filter messages to (inclusive)</param>
    /// <returns>Statistics containing message counts by type for the specified user in the chat</returns>
    [HttpGet("get-user-message-statistics")]
    public async Task<GetUserMessageStatisticsResponseDto> GetUserMessageStatisticsAsync(long chatId, long userId, DateTime? from, DateTime? to)
    {
        var result = await messageService.GetUserMessageStatisticsAsync(chatId, userId, from, to, HttpContext.RequestAborted);
        return result.ToGetUserMessageStatisticsResponse();
    }


    /// <summary>
    /// Get message statistics for all users in a specific chat
    /// </summary>
    /// <param name="chatId">The Telegram chat ID</param>
    /// <param name="from">Optional start date to filter messages from (inclusive)</param>
    /// <param name="to">Optional end date to filter messages to (inclusive)</param>
    /// <returns>Statistics containing message counts by type for all users in the specified chat</returns>
    [HttpGet("get-chat-message-statistics")]
    public async Task<GetChatMessageStatisticsResponseDto> GetChatMessageStatisticsAsync(long chatId, DateTime? from, DateTime? to)
    {
        var result = await messageService.GetChatMessageStatisticsAsync(chatId, from, to, HttpContext.RequestAborted);
        var userChatStatisticsDto = result.Select(x => x.ToGetUserMessageStatisticsResponse()).ToArray();
        return new GetChatMessageStatisticsResponseDto(userChatStatisticsDto);
    }

    /// <summary>
    /// Get user's favorite sticker in a specific chat
    /// </summary>
    /// <param name="userId">The Telegram user ID</param>
    /// <param name="chatId">The Telegram chat ID</param>
    /// <returns>The ID of the most frequently used sticker by the user in the specified chat</returns>
    [HttpGet("get-fav-sticker")]
    public async Task<GetFavStickerResponseDto> GetFavStickerAsync(long userId, long chatId)
    {
        var result = await messageService.GetFavStickerAsync(userId, chatId, HttpContext.RequestAborted);
        return new GetFavStickerResponseDto(result);
    }
}
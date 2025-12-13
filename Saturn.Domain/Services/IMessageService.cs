using Saturn.Domain.Model;

namespace Saturn.Domain.Services;

public interface IMessageService
{
    Task SaveMessageAsync(TelegramMessage telegramMessage, CancellationToken cancellationToken);
    
    Task<UserMessageStatistics> GetUserMessageStatisticsAsync(long userId, long chatId, DateTime? from, DateTime? to, CancellationToken cancellationToken); 
}
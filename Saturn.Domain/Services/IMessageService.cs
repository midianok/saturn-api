using Saturn.Domain.Model;

namespace Saturn.Domain.Services;

public interface IMessageService
{
    Task SaveMessageAsync(TelegramMessage telegramMessage);
}
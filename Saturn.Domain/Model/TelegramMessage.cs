namespace Saturn.Domain.Model;

public record TelegramMessage
{
    public long Id { get; init; }

    public DateTime MessageDate { get; init; }
    
    public int Type { get; init; }

    public string? Text { get; init; }

    public string? StickerId { get; init; }
    
    public long? ReplyToMessageId { get; init; }
    
    public long? ReplyToMessageChatId { get; init; }
    
    public TelegramUser? From { get; init; }
    
    public TelegramChat? Chat { get; init; }
}
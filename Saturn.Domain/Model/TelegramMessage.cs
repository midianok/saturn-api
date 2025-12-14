namespace Saturn.Domain.Model;

public class TelegramMessage
{
    public long Id { get; set; }

    public DateTime MessageDate { get; set; }
    
    public int Type { get; set; }

    public string? Text { get; set; }

    public string? StickerId { get; set; }
    
    public long? ReplyToMessageId { get; set; }
    
    public long? ReplyToMessageChatId { get; set; }
    
    public TelegramUser? From { get; set; }
    
    public TelegramChat? Chat { get; set; }
}
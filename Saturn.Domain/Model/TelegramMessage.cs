namespace Saturn.Domain.Model;

public class TelegramMessage
{
    public long Id { get; init; }
    public string? Text { get; init; }
    
    public DateTime Date { get; init; }
    
    public int Type { get; init; }
    
    public TelegramMessage? ReplyToMessage { get; init; }
    
    public TelegramUser? From { get; init; }
    
    public TelegramChat? Chat { get; init; }
    
    public TelegramSticker? Sticker { get; init; }
}
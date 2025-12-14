namespace Saturn.Application.Dtos;

public record SaveMessageDto
{
    public long MessageId { get; init; }
    
    public string? MessageText { get; init; }
    
    public DateTime MessageDate { get; init; }
    
    public int MessageType { get; init; }
    
    public long ChatId { get; init; }
    
    public string? ChatName { get; init; }
    
    public int ChatType { get; init; }
    
    public string? StickerId { get; init; }
    
    public long? FromId { get; init; }
    
    public string? FromFirstName { get; init; }
    
    public string? FromLastName { get; init; }
    
    public string? FromUsername { get; init; }
    
    public long? ReplyToMessageId { get; init; }
    
    public long? ReplyToMessageChatId { get; init; }
}